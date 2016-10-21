using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThoConShop.Web.Startup))]
namespace ThoConShop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
