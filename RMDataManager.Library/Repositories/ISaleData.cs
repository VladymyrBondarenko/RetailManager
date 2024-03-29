﻿using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public interface ISaleData
    {
        Task<List<SaleReportModel>> GetSaleReportModels();
        Task SaveSale(SaleTransientModel saleModel, string cashierId);
    }
}