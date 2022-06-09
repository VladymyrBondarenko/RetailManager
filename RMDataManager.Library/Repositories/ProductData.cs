﻿using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;
        private static readonly string connectionId = "RetailManagerDataConnection";

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var res = await _db.LoadData<ProductModel, dynamic>(
                "spProduct_GetAll", connectionId);
            return res;
        }
    }
}