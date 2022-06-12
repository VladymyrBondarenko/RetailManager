using RMDataManager.Library.Models;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public interface ISaleData
    {
        Task SaveSale(SaleTransientModel saleModel, string cashierId);
    }
}