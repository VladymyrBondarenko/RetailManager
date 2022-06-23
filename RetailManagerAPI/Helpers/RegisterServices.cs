using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Repositories;

namespace RetailManagerAPI.Helpers
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            builder.Services.AddTransient<IUserData, UserData>();
            builder.Services.AddTransient<IProductData, ProductData>();
            builder.Services.AddTransient<ISaleData, SaleData>();
            builder.Services.AddTransient<IInventoryData, InventoryData>();
        }
    }
}
