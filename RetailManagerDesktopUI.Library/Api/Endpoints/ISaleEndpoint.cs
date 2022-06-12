using RetailManagerDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel saleModel);
    }
}