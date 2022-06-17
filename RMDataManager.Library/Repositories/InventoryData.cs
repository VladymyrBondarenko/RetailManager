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
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _db;

        public InventoryData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<InventoryModel>> GetInventory()
        {
            var res = await _db.LoadData<InventoryModel, dynamic>(
                "dbo.spInventory_GetAll", RetailManagerDataConnectionId);
            return res.ToList();
        }

        public async Task SaveInventoryRecord(InventoryModel inventoryModel)
        {
            await _db.SaveData("dbo.spInventory_Insert", RetailManagerDataConnectionId, inventoryModel);
        }
    }
}
