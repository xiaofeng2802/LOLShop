using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.Business.Identity;
using ThoConShop.DataSeedWork;
using ThoConShop.DataSeedWork.Enum;
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
            string result = api.VerifiedCard(vm.SerialNumber, vm.PinNumber, vm.CardType, "Nạp thẻ thành công", out price);

            if (string.IsNullOrEmpty(result))
            {
                if (user != null)
                {
                    var sumOfBalance = user.Balance + price;
                    _userService.UpdateBalanceUser(User.Identity.GetUserId(), sumOfBalance);
                }

                _rechargeHistoryService.Create(new UserRechargeHistoryDto()
                {
                    CreatedDate = DateTime.Now,
                    Message = "Nap Tien Game Lien Minh",
                    ParValue = price,
                    SerialNumber = vm.SerialNumber,
                    PinNumber = vm.PinNumber,
                    UserId = user.Id,
                    CardType = vm.CardType.ToString()
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
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
           
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            var isActive = UserExternalService.GetUserStatus(loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:id").Value);
            if (isActive != null && !(bool) isActive)
            {
                LogOff();
                return RedirectToAction("LockNoticeView", "Home", new { userName = loginInfo.Email ?? loginInfo.DefaultUserName  });
            }

            switch (result)
            {
                case SignInStatus.Success:
                    returnUrl = returnUrl ?? Url.Action("Index", "Home");
                    if (returnUrl.Contains("ChargingView"))
                    {
                        returnUrl = Url.Action("ChargingView", "User");
                    }
                    else if (returnUrl.ToLower().Contains("luckywheel"))
                    {
                        returnUrl = Url.Action("LuckyWheel", "Home");
                    }
                    else if (returnUrl.ToLower().Contains("management"))
                    {
                        returnUrl = Url.Action("AccountManagement", "Management");
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

                    if (returnUrl.Contains("ChargingView"))
                    {
                        returnUrl = Url.Action("ChargingView", "User");
                    }
                    else if(returnUrl.ToLower().Contains("luckywheel"))
                    {
                        returnUrl = Url.Action("LuckyWheel", "Home");
                    }
                    else if (returnUrl.ToLower().Contains("management"))
                    {
                        returnUrl = Url.Action("AccountManagement", "Management");
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
                        //var firstName = info.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name").Value;
                        _userService.Create(new UserDto()
                        {
                            Balance = 0,
                            CreatedDate = DateTime.Now,
                            GeneralUserId = user.Id,
                            IsActive = true,
                            NameDisplay = info.DefaultUserName
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

        public JsonResult GetRandomWheelItem()
        {
            if (UserExternalService.IsEnoughPoint(User.Identity.GetUserId()))
            {
                int resultForWheel;
                var result = _userService.RandomWheelItem(out resultForWheel);
                Session["CurrentPrice"] = result.Id;
                return Json(resultForWheel, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Bạn không đủ số Point để quay, xin vui lòng nạp thêm thẻ. Cám ơn!" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveWheelHistory(int priceId = -1)
        {
            int serverValidatorPrice = (int?) Session["CurrentPrice"] ?? -1;
            int defaultPoint = int.Parse(ConfigurationManager.AppSettings["PointPerWheel"]);
            if (priceId == serverValidatorPrice)
            {
                var user = _userService.ReadByGeneralUserId(User.Identity.GetUserId());
                var sumOfPoint = user.Point - defaultPoint;
                if (UserExternalService.IsUnluckyItem(priceId))
                {
                    _userService.UpdatePointUser(user.GeneralUserId, sumOfPoint);
                    return Json("Chúc bạn may mắn lần sau !", JsonRequestBehavior.AllowGet);
                }
                var desc = UserExternalService.GetDescriptonWheelItem(priceId);
               
                LuckyWheelHistoryDto h = new LuckyWheelHistoryDto()
                {
                    CreatedDate = DateTime.Now,
                    NameDisplay = user.NameDisplay,
                    UserId = user.Id,
                    Result = desc
                };

                var result = _userService.CreateLuckyHistory(h);
                if (result != null)
                {
                    _userService.UpdatePointUser(user.GeneralUserId, sumOfPoint);
                }
                return Json("Chúc mừng bạn đã trúng " + desc + ". Xin vui lòng liên hệ fanpage để nhận quà, Cám ơn bạn.", JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = "error", message = "Xảy ra lỗi khi cập nhật thông tin, vui lòng quay số lại." }, JsonRequestBehavior.AllowGet);
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