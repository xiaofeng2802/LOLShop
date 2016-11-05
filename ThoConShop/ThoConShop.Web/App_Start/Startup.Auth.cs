using Owin;


namespace ThoConShop.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            ThoConShop.Business.Identity.IdentityConfig.Builder(app);
        }
    }
}