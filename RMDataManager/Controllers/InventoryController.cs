using Autofac;
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
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiController
    {
        private readonly IInventoryData _inventoryData;

        public InventoryController()
        {
            _inventoryData = ServiceTuner.Resolve<IInventoryData>();
        }

        [HttpGet]
        [Authorize(Roles = "Manger,Admin")]
        public async Task<List<InventoryModel>> Get()
        {
            var res = await _inventoryData.GetInventory();
            return res.ToList();
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task Post(InventoryModel inventoryModel)
        {
            await _inventoryData.SaveInventoryRecord(inventoryModel);
        }
    }
}