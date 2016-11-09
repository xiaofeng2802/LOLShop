using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    public class ManagementController : Controller
    {
        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private readonly IAccountService _accountService;

        private readonly IRankService _rankService;


        public ManagementController(IAccountService accountService, IRankService rankService)
        {
            _accountService = accountService;
            _rankService = rankService;
        }



        // GET: Management
        public ActionResult AccountManagement(int? page)
        {
            var result = _accountService.Read(page ?? 1, _pageSize, false);

            return View(result);
        }

        public ActionResult CreateAccount()
        {
            CreateOrUpdateAccountViewModel vm = new CreateOrUpdateAccountViewModel()
            {
                RankList = _rankService.Read().Select(a => new SelectListItem()
                {
                    Value = a.Id.ToString(),
                    Text = a.RankName
                }).ToList(),

            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(CreateOrUpdateAccountViewModel data)
        {
            if (ModelState.IsValid)
            {
                var result = new AccountDto()
                {
                    UserName = data.UserName,
                    Password = data.Password,
                    CreatedDate = DateTime.Now,
                    Description = data.Description,
                    Price = data.Price,
                    RankId = data.RankId,
                    Avatar = ""
                };
                _accountService.Create(result);
                return RedirectToAction("AccountManagement");
            }
            return View(data);
        }

        public ActionResult UserManagement()
        {
            return View();
        }

        public ActionResult RankManagement()
        {
            return View();
        }

        public ActionResult ChargingHistories()
        {
            return View();
        }

        public ActionResult TradingHistories()
        {
            return View();
        }

        public ActionResult FeedManagement()
        {
            return View();
        }
    }
}