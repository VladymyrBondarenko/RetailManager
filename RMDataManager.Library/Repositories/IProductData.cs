using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDataManager.Library.Repositories
{
    public interface IProductData
    {
        Task<ProductModel> GetProductById(int id);
        Task<List<ProductModel>> GetProducts();
    }
}