using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ThoConShop.DataSeedWork.Identity
{

    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        
    }
}
