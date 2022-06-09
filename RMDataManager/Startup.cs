using Autofac;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RMDataManager.Startup))]

namespace RMDataManager
{
    public partial class Startup
    {
        internal static IContainer ServiceTuner { get; set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
