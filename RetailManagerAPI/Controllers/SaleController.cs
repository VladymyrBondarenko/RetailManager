using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.Models;
using RMDataManager.Library.Repositories;
using System.Security.Claims;

namespace RetailManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData)
        {
            _saleData = saleData;
        }

        [HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetSalesReport()
        {
            var res = await _saleData.GetSaleReportModels();
            return Ok(res.ToList());
        }

        [HttpPost()]
        [Authorize(Roles = "Cashier")]
        public async Task PostAsync(SaleTransientModel saleModel)
        {
            await _saleData.SaveSale(saleModel, User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
