using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoConShop.Business.Contracts;

namespace ThoConShop.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IAccountService accountService;

        public HomeController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        public ActionResult Index()
        {
            var result = accountService.Read();
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}