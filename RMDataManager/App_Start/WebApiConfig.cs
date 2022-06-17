using System.Web.Http;
using Autofac;
using Microsoft.Owin.Security.OAuth;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Repositories;
using static RMDataManager.Startup;

namespace RMDataManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var builder = new ContainerBuilder();
            builder.RegisterType<SqlDataAccess>().As<ISqlDataAccess>();
            builder.RegisterType<UserData>().As<IUserData>();
            builder.RegisterType<ProductData>().As<IProductData>();
            builder.RegisterType<SaleData>().As<ISaleData>();
            builder.RegisterType<InventoryData>().As<IInventoryData>();

            ServiceTuner = builder.Build();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
