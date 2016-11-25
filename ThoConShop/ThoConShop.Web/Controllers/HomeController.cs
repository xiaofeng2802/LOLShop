﻿using System;
using System.Collections.Generic;
using System.Configuration;
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

        public ActionResult Index(int? page, int? currentRankFilter, string currentPriceFilter, string currentSkinFilter ="")
        {
            int pageIndex = page ?? 1;

            AccountIndexViewModel viewModel = new AccountIndexViewModel()
            {
                DataSource = Filter(pageIndex, currentRankFilter, currentPriceFilter, currentSkinFilter),
                RanksFilter = _accountRelationDataService.ReadRankForFilter(),
                PriceFilter = _accountRelationDataService.ReadPriceRangeForFilter(),
                CurrentRankFilter = currentRankFilter,
                CurrentPriceFilter = currentPriceFilter,
                CurrentSkinFilter = currentSkinFilter
            };

            if (Session["NewsLoaded"] == null)
            {
                var path = Server.MapPath("~/New.txt");
                viewModel.News = NewExternalService.ReadNew(path);
                Session["NewsLoaded"] = true;
            }

            return View(viewModel);
        }


        public ActionResult Edit(int accountId = 0)
        {
            var result = _accountService.Edit(accountId);
            if (result != null)
            {
                float fromValue = (result.Price - 300000)
                   , toValue = (result.Price + 300000);
                fromValue = fromValue < 0 ? 0 : fromValue;

                AccountEditViewModel vm = new AccountEditViewModel
                {
                    CurrentAccount = result,
                    SuggestionAccounts = _accountRelationDataService.ReadAccountByPriceRange(fromValue, toValue, new List<int>() { accountId })
                };

                return View(vm);
            }

            return HttpNotFound("Account could not be found.");
        }

        private IPagedList<AccountDto> Filter(int pageIndex, int? currentRankFilter, string currentPriceFilter, string currentSkinFilter)
        {
            IPagedList<AccountDto> result;
            if (currentRankFilter == null && string.IsNullOrEmpty(currentPriceFilter) && currentSkinFilter == null)
            {
                result = _accountService.Read(pageIndex, _pageSize);
            }
            else
            {
                result = _accountService.FilterByRankPriceSkin(pageIndex, _pageSize, currentRankFilter, currentPriceFilter, currentSkinFilter);
            }
            return result;
        }

        [Authorize]
        public ActionResult LuckyWheel(int page = 1)
        {
            var result = _userService.ReadLuckyWheelHistory(page, _pageSize);
            return View(result);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}