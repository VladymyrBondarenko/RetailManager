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

            try
            {
                using (var transactionDataAccess = _db.StartTransaction(connectionId))
                {
                    try
                    {
                        await transactionDataAccess.SaveDataInTransaction("dbo.spSale_Insert", sale);

                        var res = await transactionDataAccess.LoadDataInTransaction<int, dynamic>(
                            "spSale_Lookup",
                            new { sale.CashierId, sale.SaleDate });
                        var saleId = res.FirstOrDefault();

                        foreach (var detail in details)
                        {
                            detail.SaleId = saleId;

                            await transactionDataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", detail);
                        }

                        transactionDataAccess.CommitTransaction();
                    }
                    catch
                    {
                        transactionDataAccess.RollbackTransaction();
                        throw;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
