using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoConShop.Business.Contracts;

namespace ThoConShop.Web.Controllers
{
    public class ManagementController : Controller
    {
        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private readonly IAccountService _accountService;


        public ManagementController(IAccountService accountService)
        {
            _accountService = accountService;
        }



        // GET: Management
        public ActionResult AccountManagement(int? page)
        {
            var result = _accountService.Read(page ?? 1, _pageSize, false);

            return View(result);
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