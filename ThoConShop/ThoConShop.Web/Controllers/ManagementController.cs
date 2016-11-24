using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using AutoMapper;
using Newtonsoft.Json.Linq;
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

                if (accountDto == null)
                {
                    return HttpNotFound("");
                }

                vm = new CreateOrUpdateAccountViewModel()
                {
                    RankList = _rankService.Read().Select(a => new SelectListItem()
                    {
                        Value = a.Id.ToString(),
                        Text = a.RankName,
                        Selected = (a.Id == accountDto.RankId)
                    }).ToList(),
                    Price = accountDto.Price,
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
                    Price = data.Price,
                    RankId = data.RankId,
                    Avatar = ConfigurationManager.AppSettings["AvatarUrl"] + FileUlti.SaveFile(data.Avatar, path),
                    IsAvailable = true,
                    IsHot = true
                };
                createOrpdateResult = _accountService.Create(result, FileUlti.ReadFromTextFile(data.Champs), FileUlti.ReadFromTextFile(data.Skins));
            }
            else
            {
                var accountDto = _accountService.ReadOneById(data.AccounId);
                var url = FileUlti.SaveFile(data.Avatar, path);
                accountDto.Price = data.Price;
                accountDto.Description = data.Description;
                accountDto.Password = data.Password;
                accountDto.UserName = data.UserName;
                accountDto.RankId = data.RankId;
                accountDto.Avatar = (url == null ? accountDto.Avatar : ConfigurationManager.AppSettings["AvatarUrl"] + url);
                
                if (data.PageGem != null)
                {
                    _accountRelationDataService.DeletePageGemByAccountId(data.AccounId);
                }
                createOrpdateResult = _accountService.Update(accountDto, FileUlti.ReadFromTextFile(data.Champs), FileUlti.ReadFromTextFile(data.Skins));

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
                            ImageUrl = ConfigurationManager.AppSettings["PageGemUrl"] + FileUlti.SaveFile(item, pathPageGem)
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


        public ActionResult UserManagement(int? page, string searchString = "")
        {
            var result = _userService.Read(page ?? 1, _pageSize, searchString);
            ViewBag.SearchString = searchString;
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

        public ActionResult RankManagement(int? page, string searchString)
        {
            var result = _rankService.Read(page ?? 1, _pageSize, searchString: searchString);
            return View(result);
        }

        public ActionResult CreateRankView()
        {
            CreateRankViewModel vm = new CreateRankViewModel()
            {
               
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRank(CreateRankViewModel data)
        {
            var path = Server.MapPath("~/Images");
            var rankdto = new RankDto()
            {
                CreatedDate = DateTime.Now,
                RankName = data.RankName,
                RankImage = "../Images/" + FileUlti.SaveFile(data.RankImage, path)
            };

            _rankService.Create(rankdto);

            return RedirectToAction("RankManagement");
        }

        public ActionResult DeleteRank(int rankId = 0)
        {
            try
            {
                _rankService.Delete(rankId);
            }
            catch (Exception)
            {
            }
            
            return RedirectToAction("RankManagement");
        }

        public ActionResult ChargingHistories(UserRechargeViewModel data, int page = 1)
        {
            data.ReportMonth = data.ReportMonth == 0 ? DateTime.Now.Month : data.ReportMonth;
            var result = _accountRelationDataService.ReadRechargeHistories(page, _pageSize, data.ReportMonth);

            UserRechargeViewModel vm = new UserRechargeViewModel
            {
                DataSource = result,
                ReportMonth = data.ReportMonth
            };
            return View(vm);
        }

        public ActionResult TradingHistories(int page = 1)
        {
            var data = _accountRelationDataService.ReadTradingHistories(page, _pageSize);
            return View(data);
        }

        public ActionResult FeedManagement()
        {
            return View();
        }

        public ActionResult ChampManagement(int page = 1)
        {
            var result = _accountRelationDataService.ReadChamp(page, _pageSize);
            return View(result);
        }

        [HttpPost]
        public ActionResult UploadChamp(HttpPostedFileBase champFull)
        {
            StreamReader reader = new StreamReader(champFull.InputStream);
            string json = reader.ReadToEnd();

            try
            {
                _accountRelationDataService.UploadDataFromJson(json);
            }
            catch (Exception ex)
            {
                  
            }
           

            //var dataa = (Dictionary<string, ChampUploadDto>)data["data"];
            return RedirectToAction("ChampManagement");
        }


        public ActionResult DeleteChamp(int champId = 0)
        {
            _accountRelationDataService.DeleteChamp(champId);
            return RedirectToAction("ChampManagement");
        }

        public ActionResult SkinManagement(int page = 1)
        {
            var result = _accountRelationDataService.ReadSkin(page, _pageSize);
            return View(result);
        }

        public ActionResult DeleteSkin(int skinId = 0)
        {
            _accountRelationDataService.DeleteSkin(skinId);
            return RedirectToAction("SkinManagement");
        }

        public JsonResult ChampDataSource(int accountId = 0)
        {
            if (accountId > 0)
            {
                var result = _accountRelationDataService.ReadChamp().Select(a => a.ChampionName);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SkinDataSource(int accountId = 0)
        {
            if (accountId > 0)
            {
                var result = _accountRelationDataService.ReadSkin(isParentOnly: false).Select(a => a.SkinName);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WheelManagement(int? page)
        {
            var result = _userService.ReadLuckyWheelItem(page ?? 1, _pageSize);

            return View(result);
        }

        [HttpPost]
        public ActionResult CreateWheelItem(LuckyWheelItemDto data)
        {
            data.ImageUrl = Server.MapPath("../Images/LuckyItem/");
            var result = _userService.CreateLuckyItem(data);
            return RedirectToAction("WheelManagement");
        }

        public ActionResult CreateWheelItem()
        {
            LuckyWheelItemDto vm = new LuckyWheelItemDto()
            {
            };
            return View(vm);
        }

        public ActionResult DeleteWheelItem(int id = 0)
        {
            var result = _userService.DeleteLuckyItem(id);
            return RedirectToAction("WheelManagement");
        }

        public JsonResult ReadAllWheelItem()
        {
            var result = _userService.ReadAllLuckyWheelItem().Select(a => new
            {
                id = a.Id,
                description = a.Description,
                text= a.DisplayName,
                image = a.ImageUrl,
                fillStyle = "#7de6ef"
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}