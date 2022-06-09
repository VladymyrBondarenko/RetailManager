using RetailManagerDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api.Endpoints
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}