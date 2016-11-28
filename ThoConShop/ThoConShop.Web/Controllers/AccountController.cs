using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ThoConShop.Business.Contracts;
using ThoConShop.DataSeedWork.UserExternalService;
using ThoConShop.Web.Models;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        private readonly IHistoryService _historyService;

        private readonly IUserService _userService;
        public AccountController(IAccountService accountService,
            IHistoryService historyService,
            IUserService userService)
        {
            _accountService = accountService;
            _historyService = historyService;
            _userService = userService;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellAccount(int accountId)
        {

            var account = _accountService.ReadOneById(accountId);
            var user = _userService.ReadByGeneralUserId(User.Identity.GetUserId());

            if (account.Price > UserExternalService.GetUserBalance(User.Identity.GetUserId()))
            {
                return RedirectToAction("ChargingView", "User",
                    new {Message = "Tài khoản quý khách không đủ tiền, xin vui lòng nạp thêm. Cám ơn !"});
            }

            _accountService.SoldAccount(accountId);
            if (user != null)
            {
                var sumOfBalance = user.Balance - account.Price;
                _userService.UpdateBalanceUser(user.GeneralUserId, sumOfBalance);

                _historyService.Create(new UserTradingHistoryDto()
                {
                    AccountId = account.Id,
                    UserId = user.Id,
                    CreatedDate = DateTime.Now
                });
            }

           

            AccountSoldViewModel vm = new AccountSoldViewModel()
            {
                AccountName = account.UserName,
                Password = account.Password,
                Description = account.Description
            };
            return View(vm);
        }
    }
}

