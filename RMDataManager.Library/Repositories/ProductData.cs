using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RMDataManager.Library.Internal.Settings.DbConfiguration;

namespace RMDataManager.Library.Repositories
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var res = await _db.LoadData<ProductModel, dynamic>(
                "spProduct_GetAll", RetailManagerDataConnectionId);
            return res;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var res = await _db.LoadData<ProductModel, dynamic>(
                "spProduct_GetById", RetailManagerDataConnectionId, new { id = id });
            return res.FirstOrDefault();
        }
    }
}