using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ThoConShop.Business.Contracts;
using ThoConShop.DataSeedWork.UserExternalService;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellAccount(int accountId)
        {

            var account = _accountService.ReadOneById(accountId);

            if (account.Price > (decimal) UserExternalService.GetUserBalance(User.Identity.GetUserId()))
            {
                return RedirectToAction("ChargingView", "User",
                    new {Message = "Tài khoản quý khách không đủ tiền, xin vui lòng nạp thêm. Cám ơn !"});
            }

            _accountService.SoldAccount(accountId);
            UserExternalService.SetUserBalance(User.Identity.GetUserId(), (account.Price * -1));
            AccountSoldViewModel vm = new AccountSoldViewModel()
            {
                AccountName = account.AccountName,
                Password = account.Password
            };
            return View(vm);
        }
    }
}