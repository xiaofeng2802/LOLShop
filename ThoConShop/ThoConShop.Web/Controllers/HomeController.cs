using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        readonly IAccountService _accountService;
        private readonly IAccountRelationDataService _accountRelationDataService;

        public HomeController(IAccountService accountService, IAccountRelationDataService accountRelationDataService)
        {
            _accountService = accountService;
            _accountRelationDataService = accountRelationDataService;
        }

        public ActionResult Index(int? page, int? currentGankFilter, string currentPriceFilter, int? currentSkinFilter)
        {
            int pageIndex = page ?? 1;
            IPagedList<AccountDto> result;

            if (currentGankFilter == null && string.IsNullOrEmpty(currentPriceFilter) && currentSkinFilter == null)
            {
                result = _accountService.Read(pageIndex, _pageSize);
            }
            else
            {
                result = _accountService.FilterByGankPriceSkin(pageIndex, _pageSize, currentGankFilter, currentPriceFilter, currentSkinFilter);
            }

            AccountIndexViewModel viewModel = new AccountIndexViewModel()
            {
                DataSource = result,
                GanksFilter = _accountRelationDataService.ReadGankForFilter(),
                SkinsFilter = _accountRelationDataService.ReadSkinForFilter(),
                PriceFilter = _accountRelationDataService.ReadPriceRangeForFilter(),
                CurrentGankFilter = currentGankFilter,
                CurrentPriceFilter = currentPriceFilter,
                CurrentSkinFilter = currentSkinFilter
            };
            return View(viewModel);
        }
    }
}