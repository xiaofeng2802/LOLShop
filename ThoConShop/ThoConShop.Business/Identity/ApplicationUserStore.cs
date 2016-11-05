using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThoConShop.DataSeedWork.Identity;

namespace ThoConShop.Business.Identity
{
    public class ApplicationUserStore: UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>,
    IDisposable, IUserStore<ApplicationUser>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
        }
    }
}
