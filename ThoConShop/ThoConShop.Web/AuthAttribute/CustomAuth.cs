using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Services;
using ThoConShop.DataSeedWork.UserExternalService;

namespace ThoConShop.Web.AuthAttribute
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        //Custom named parameters for annotation
        public string ResourceKey { get; set; }
        public string OperationKey { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
           
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        //Called when access is denied
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //User isn't logged in
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string controller = filterContext.Controller.ToString();

                if (controller.Contains("User"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "User",
                                action = "ExternalLogin",
                                returnUrl =
                                string.Format(@"\{0}\{1}", "User",
                                    filterContext.ActionDescriptor.ActionName)
                            })
                    );
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new {controller = "User", action = "ExternalLogin"})
                    );
                }
               
            }
            //User is logged in but has no access
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Home", action = "LockNoticeView" })
                );
            }
        }
    }
}