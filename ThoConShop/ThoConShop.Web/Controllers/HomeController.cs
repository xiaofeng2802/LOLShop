using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.NewsService;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

        readonly IAccountService _accountService;
        private readonly IAccountRelationDataService _accountRelationDataService;
        private readonly IUserService _userService;

        public HomeController(IAccountService accountService, IAccountRelationDataService accountRelationDataService,
            IUserService userService)
        {
            _accountService = accountService;
            _accountRelationDataService = accountRelationDataService;
            _userService = userService;
        }

        [AllowAnonymous]
        public ActionResult LockNoticeView(string userName)
        {
            return View("LockNoticeView", (object)userName);
        }

        [Route("/trangchu/{}")]
        public ActionResult Index(int? page, int? currentRankFilter, string currentPriceFilter, string currentSkinFilter, string currentChampFilter, string currentOrderBy)
        {
            int pageIndex = page ?? 1;

            AccountIndexViewModel viewModel = new AccountIndexViewModel()
            {
                DataSource = Filter(pageIndex, currentRankFilter, currentPriceFilter, currentSkinFilter, currentChampFilter, currentOrderBy),
                RanksFilter = _accountRelationDataService.ReadRankForFilter(),
                PriceFilter = _accountRelationDataService.ReadPriceRangeForFilter(),
                CurrentRankFilter = currentRankFilter,
                CurrentPriceFilter = currentPriceFilter,
                CurrentSkinFilter = currentSkinFilter,
                CurrentChampFilter = currentChampFilter,
                CurrentOrderBy = currentOrderBy
            };

            if (Session["NewsLoaded"] == null)
            {
                var path = Server.MapPath("~/New.txt");
                viewModel.News = NewExternalService.ReadNew(path);
                Session["NewsLoaded"] = true;
            }

            var pathRandom = Server.MapPath("~/Images/RandomImage");
            SetRandomImage(pathRandom, viewModel.DataSource);

            return View(viewModel);
        }


        public ActionResult Edit(int accountId = 0)
        {
            var result = _accountService.Edit(accountId);
            if (result != null)
            {
                AccountEditViewModel vm = new AccountEditViewModel
                {
                    CurrentAccount = result,
                    SuggestionAccounts = _accountRelationDataService.ReadAccountBySamePrice(result.Price, new List<int>() { accountId })
                };

                return View(vm);
            }

            return HttpNotFound("Account could not be found.");
        }

        private IPagedList<AccountDto> Filter(int pageIndex, int? currentRankFilter, string currentPriceFilter, string currentSkinFilter, string currentChampFilter, string currentOrderBy)
        {
            IPagedList<AccountDto> result;
            if (currentRankFilter == null && string.IsNullOrEmpty(currentPriceFilter) && currentSkinFilter == null && string.IsNullOrEmpty(currentChampFilter) && string.IsNullOrEmpty(currentOrderBy))
            {
                result = _accountService.Read(pageIndex, _pageSize);
            }
            else
            {
                result = _accountService.FilterByRankPriceSkin(pageIndex, _pageSize, currentRankFilter, currentPriceFilter, currentSkinFilter, currentChampFilter, currentOrderBy);
            }
            return result;
        }

        [Authorize]
        public ActionResult LuckyWheel(int page = 1)
        {
            if (ConfigurationManager.AppSettings["IsWheelOpen"] != null 
                && ConfigurationManager.AppSettings["IsWheelOpen"] == "1")
            {
                var result = _userService.ReadLuckyWheelHistory(page, _pageSize);
                return View(result);
            }
            return RedirectToAction("NotReady");
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult NotReady()
        {
            return View();
        }

        private void SetRandomImage(string path, IPagedList<AccountDto>  source)
        {
            if (!string.IsNullOrEmpty(path) && source != null)
            {
                var randomImageList = Directory.GetFiles(path, "*.jpg").Select(a => a.Substring(a.LastIndexOf("\\", StringComparison.Ordinal) + 1)).ToList();
                Random r = new Random();

                foreach (var item in source)
                {

                    item.Avatar = "../Images/RandomImage/" +
                                  randomImageList[r.Next(0, randomImageList.Count - 1)];
                }
            }
        }
    }
}