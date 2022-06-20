using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.Models;
using RMDataManager.Library.Repositories;

namespace RetailManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData _inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        [HttpGet]
        [Authorize(Roles = "Manger,Admin")]
        public async Task<IActionResult> Get()
        {
            var res = await _inventoryData.GetInventory();
            return Ok(res.ToList());
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task Post(InventoryModel inventoryModel)
        {
            await _inventoryData.SaveInventoryRecord(inventoryModel);
        }
    }
}
