using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.Business.Identity;
using ThoConShop.DataSeedWork;
using ThoConShop.DataSeedWork.Identity;
using ThoConShop.DataSeedWork.UserExternalService;
using ThoConShop.Web.AuthAttribute;
using ThoConShop.Web.GameBank;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private readonly IUserService _userService;

        private readonly IAccountRelationDataService _accountRelationDataService;

        private readonly IHistoryService _rechargeHistoryService;

        public UserController(IUserService userService,
            IAccountRelationDataService accountRelationDataService,
             IHistoryService rechargeHistoryService)
        {
            _userService = userService;
            _accountRelationDataService = accountRelationDataService;
            _rechargeHistoryService = rechargeHistoryService;
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult ViewHistoriesUser()
        {

            var result = new HistoryViewModel() {
                RechargeHistories = _accountRelationDataService.ReadRechargeHistories(User.Identity.GetUserId(), 1, _pageSize),
                TradingHistories = _accountRelationDataService.ReadTradingHistories(User.Identity.GetUserId(), 1, _pageSize)
            };

            return View(result);
        }

        [CustomAuthorize]
        public ActionResult ChargingView(string message)
        {
            ViewBag.Message = message;
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChargingView(ChargingViewModel vm)
        {
            var userId = User.Identity.GetUserId();
            UserDto user = _userService.ReadByGeneralUserId(userId);
            GameBankAPI api = new GameBankAPI();
            int price;
            string result = api.VerifiedCard(vm.SerialNumber, vm.PinNumber, vm.CardType, "Nap Tien Game Lien Minh", out price);
            if (string.IsNullOrEmpty(result))
            {
                UserExternalService.SetUserBalance(userId, price);
                _rechargeHistoryService.Create(new UserRechargeHistoryDto()
                {
                    CreatedDate = DateTime.Now,
                    Message = "Nap Tien Game Lien Minh",
                    ParValue = price,
                    SerialNumber = vm.SerialNumber,
                    PinNumber = vm.PinNumber,
                    UserId = user.Id
                });

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", result);
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result =
                await
                    SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                        shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = model.RememberMe});
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe,
                        rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "User");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "User");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider = "Facebook")
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "User", new {ReturnUrl = Url.Action("Index", "Home") }));
        }



        [AllowAnonymous]
        public ActionResult ExternalLogin(string returnUrl, string provider = "Facebook")
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "User", new { ReturnUrl = Url.Action("Index", "Home", new { ReturnUrl = returnUrl }) }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe});
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    await UserManager.AddLoginAsync(loginInfo.ExternalIdentity.GetUserId(), loginInfo.Login);
            //}
            //loginInfo.Email =  "01203195108";
            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    returnUrl = returnUrl ?? Url.Action("Index", "Home");
                    if (returnUrl != null && returnUrl.Contains("ChargingView"))
                    {
                        returnUrl = Url.Action("ChargingView", "User");
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                case SignInStatus.Failure:
                default:
                    //if (string.IsNullOrEmpty(loginInfo.Email))
                    //{
                    //    return RedirectToAction("ExternalLoginConfirmationFacebook", new { email = "", returnUrl="" });
                    //}
                    // If the user does not have an account, then prompt the user to create an account

                    returnUrl = returnUrl ?? Url.Action("Index", "Home");
                    if (returnUrl != null && returnUrl.Contains("ChargingView"))
                    {
                        returnUrl = Url.Action("ChargingView", "User");
                    }
                    return await ExternalLoginConfirmation(returnUrl);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel userInfor, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                //model.
                //info.ExternalIdentity.
                var user = new ApplicationUser { UserName = userInfor.Email, Email = userInfor.Email };
                if (user.Id == null)
                {
                    user.Id = info.ExternalIdentity.GetUserId();
                }
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var firstName = info.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name").Value;
                    _userService.Create(new UserDto()
                    {
                        Balance = 0,
                        CreatedDate = DateTime.Now,
                        GeneralUserId = user.Id,
                        IsActive = true,
                        IsDeleted = false,
                        NameDisplay = firstName
                    });
                    //result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl ?? Url.Action("Index", "Home"));
                    }
                }
                AddErrors(result);
            }
            return RedirectToAction("ExternalLoginFailure");
        }


        //
        // POST: /Account/ExternalLoginConfirmation
        public async Task<ActionResult> ExternalLoginConfirmation(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                //model.
                //info.ExternalIdentity.
                var user = new ApplicationUser { UserName = info.Email ?? info.DefaultUserName, Email = info.Email };
                if (user.Id == null)
                {
                    user.Id = info.ExternalIdentity.GetUserId();
                }
                
                if (_userService.ReadByGeneralUserId(info.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:id").Value) == null)
                {
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        var firstName = info.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name").Value;
                        _userService.Create(new UserDto()
                        {
                            Balance = 0,
                            CreatedDate = DateTime.Now,
                            GeneralUserId = user.Id,
                            IsActive = true,
                            IsDeleted = false,
                            NameDisplay = firstName
                        });
                        var data = await UserManager.GetLoginsAsync(user.Id);
                        if (data == null)
                        {
                            result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        }

                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl ?? Url.Action("Index", "Home"));
                        }
                    }
                    AddErrors(result);
                }
                else
                {
                    var data = await UserManager.GetLoginsAsync(user.Id);
                    IdentityResult result;
                    if (data == null)
                    {
                        result =  await UserManager.AddLoginAsync(user.Id, info.Login);  
                    }

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl ?? Url.Action("Index", "Home"));
                }

                
            }
            return RedirectToAction("ExternalLoginFailure");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}