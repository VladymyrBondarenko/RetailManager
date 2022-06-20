using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Repositories;

namespace RetailManagerAPI.Helpers
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            builder.Services.AddSingleton<IUserData, UserData>();
            builder.Services.AddSingleton<IProductData, ProductData>();
            builder.Services.AddSingleton<ISaleData, SaleData>();
            builder.Services.AddSingleton<IInventoryData, InventoryData>();
        }
    }
}
