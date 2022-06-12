using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess _db;
        private readonly IProductData _productData;
        private static readonly string connectionId = "RetailManagerDataConnection";

        public SaleData(ISqlDataAccess db, IProductData productData)
        {
            _db = db;
            _productData = productData;
        }

        public async Task SaveSale(SaleTransientModel saleModel, string cashierId)
        {
            var details = new List<SaleDetailModel>();
            var tax = ConfigHelper.GetTaxRate()/100;

            foreach (var saleDetail in saleModel.SaleDetailModels)
            {
                var detail = new SaleDetailModel
                {
                    ProductId = saleDetail.ProductId,
                    Quantity = saleDetail.Quantity
                };

                var product = await _productData.GetProductById(detail.ProductId);

                if (product == null)
                {
                    throw new Exception($"The product id of { detail.ProductId } could not be found.");
                }

                detail.PurchasePrice = product.RetailPrice * detail.Quantity;

                if (product.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * tax;
                }

                details.Add(detail);
            }

            var sale = new SaleModel
            {
                SubTotal = details.Sum(i => i.PurchasePrice),
                Tax = details.Sum(i => i.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.SubTotal + sale.Tax;

            await _db.SaveData("dbo.spSale_Insert", connectionId, sale);

            var res = await _db.LoadData<int, dynamic>(
                "spSale_Lookup", 
                connectionId, 
                new { sale.CashierId, sale.SaleDate });
            var saleId = res.FirstOrDefault();

            foreach (var detail in details)
            {
                detail.SaleId = saleId;

                await _db.SaveData("dbo.spSaleDetail_Insert", connectionId, detail);
            }
        }
    }
}
