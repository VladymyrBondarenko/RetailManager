﻿using Autofac;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.Models;
using RMDataManager.Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static RMDataManager.Startup;

namespace RMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiController
    {
        private readonly ISaleData _saleData;

        public SaleController()
        {
            _saleData = ServiceTuner.Resolve<ISaleData>();
        }

        [HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<List<SaleReportModel>> GetSalesReport()
        {
            var res = await _saleData.GetSaleReportModels();
            return res.ToList();
        }

        [HttpPost()]
        [Authorize(Roles = "Cashier")]
        public async Task PostAsync(SaleTransientModel saleModel)
        {
            await _saleData.SaveSale(saleModel, RequestContext.Principal.Identity.GetUserId());
        }
    }
}