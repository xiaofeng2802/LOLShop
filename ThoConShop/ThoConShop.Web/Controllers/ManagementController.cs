﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.Extensions;
using ThoConShop.DataSeedWork.Ulti;
using ThoConShop.Web.Models;

namespace ThoConShop.Web.Controllers
{
    public class ManagementController : Controller
    {
        readonly int _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        private readonly IAccountService _accountService;

        private readonly IAccountRelationDataService _accountRelationDataService;

        private readonly IRankService _rankService;

        private readonly IUserService _userService;

        public ManagementController(IAccountService accountService, IRankService rankService,
            IAccountRelationDataService accountRelationDataService,
            IUserService userService)
        {
            _accountService = accountService;
            _rankService = rankService;
            _accountRelationDataService = accountRelationDataService;
            _userService = userService;
        }



        // GET: Management
        public ActionResult AccountManagement(int? page)
        {
            var result = _accountService.Read(page ?? 1, _pageSize, false);

            return View(result);
        }

        public ActionResult CreateOrUpdateAccount(int? accountId)
        {
            CreateOrUpdateAccountViewModel vm;

            if (accountId == null)
            {
                vm = new CreateOrUpdateAccountViewModel()
                {
                    RankList = _rankService.Read().Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.RankName
                    }).ToList()

                };
            }
            else
            {
                var accountDto = _accountService.ReadOneById(accountId ?? 0);
                vm = new CreateOrUpdateAccountViewModel()
                {
                    RankList = _rankService.Read().Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.RankName,
                        Selected = (a.Id == accountDto.RankId) 
                    }).ToList(),
                    Price = (double)accountDto.Price,
                    Description = accountDto.Description,
                    UserName = accountDto.UserName,
                    Password = accountDto.Password,
                    RankId = accountDto.RankId,
                    AccounId = accountDto.Id
                    
                };
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrUpdateAccount(CreateOrUpdateAccountViewModel data)
        {

            var path = Server.MapPath(ConfigurationManager.AppSettings["AvatarUrl"]);
            AccountDto createOrpdateResult;
            if (data.AccounId == 0)
            {
                var result = new AccountDto()
                {
                    UserName = data.UserName,
                    Password = data.Password,
                    CreatedDate = DateTime.Now,
                    Description = data.Description,
                    Price = (decimal) data.Price,
                    RankId = data.RankId,
                    Avatar = FileUlti.SaveFile(data.Avatar, path),
                    IsAvailable = true,
                    IsHot = true
                };
                createOrpdateResult = _accountService.Create(result);
            }
            else
            {
                var accountDto = _accountService.ReadOneById(data.AccounId);

                accountDto.Price = (decimal) data.Price;
                accountDto.Description = data.Description;
                accountDto.Password = data.Password;
                accountDto.UserName = data.UserName;
                accountDto.RankId = data.RankId;
                accountDto.Avatar = FileUlti.SaveFile(data.Avatar, path) ?? accountDto.Avatar;

                if (data.PageGem != null)
                {
                    _accountRelationDataService.DeletePageGemByAccountId(data.AccounId);
                }
                createOrpdateResult = _accountService.Update(accountDto);

            }

            if (createOrpdateResult != null)
            {
                var pathPageGem = Server.MapPath(ConfigurationManager.AppSettings["PageGemUrl"]);
                if (data.PageGem != null)
                {
                    foreach (var item in data.PageGem)
                    {
                        var pageGem = new PageGemDto()
                        {
                            AccountId = createOrpdateResult.Id,
                            CreatedDate = DateTime.Now,
                            ImageUrl = FileUlti.SaveFile(item, pathPageGem)
                        };

                        _accountRelationDataService.CreatePageGem(pageGem);
                    }
                }
            }
            return RedirectToAction("AccountManagement");
        }

        public ActionResult DeleteAccount(int accountId = 0)
        {
            _accountService.Delete(accountId);
            return RedirectToAction("AccountManagement");
        }


        public ActionResult UpdateSkinAccount(int accountId = 0)
        {
            return View();
        }


        public ActionResult UpdateChampAccount(int accountId = 0)
        {
            return View();
        }






        public ActionResult UserManagement(int? page)
        {
            var result = _userService.Read(page ?? 1, _pageSize);
            return View(result);
        }

        public ActionResult DeactiveOrActiveUser(int userId = 0)
        {
            if (_userService.DeactiveOrActive(userId) > 0)
            {
                return RedirectToAction("UserManagement");
            }
            return null;
        }

        public ActionResult DeleteUser(int userId = 0)
        {
            if (_userService.Delete(userId) > 0)
            {
                return RedirectToAction("UserManagement");
            }
            return null;
        }

        public ActionResult RankManagement(int? page)
        {
            var result = _rankService.Read(page ?? 1, _pageSize);
            return View(result);
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