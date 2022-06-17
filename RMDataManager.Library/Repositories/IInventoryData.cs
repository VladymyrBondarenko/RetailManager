using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public interface IInventoryData
    {
        Task<List<InventoryModel>> GetInventory();
        Task SaveInventoryRecord(InventoryModel inventoryModel);
    }
}