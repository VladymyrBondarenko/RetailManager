using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RMDataManager.Library.Internal.Settings.DbConfiguration;

namespace RMDataManager.Library.Repositories
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess _db;
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;

        public SaleData(ISqlDataAccess db, IProductData productData, IConfiguration configuration)
        {
            _db = db;
            _productData = productData;
            _configuration = configuration;
        }

        public async Task SaveSale(SaleTransientModel saleModel, string cashierId)
        {
            var taxStr = string.Join(",", _configuration["taxRate"]);
            if(!decimal.TryParse(taxStr, out decimal tax) || tax == 0)
            {
                throw new Exception("The tax rate is not set up properly");
            }

            var details = new List<SaleDetailModel>();
            tax /= 100;
            
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
                using (var transactionDataAccess = _db.StartTransaction(RetailManagerDataConnectionId))
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

        public async Task<List<SaleReportModel>> GetSaleReportModels()
        {
            var res =  await _db.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", RetailManagerDataConnectionId);
            return res.ToList();
        }
    }
}
