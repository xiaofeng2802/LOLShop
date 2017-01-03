using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
using AutoMapper.Configuration;
using Newtonsoft.Json;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.Ulti;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class AccountRelationDataService : IAccountRelationDataService
    {
        readonly IRepositories<int, Rank> _repoGank;

        readonly IRepositories<int, Skin> _repoSkin;

        readonly IRepositories<int, Account> _repoAccount;

        readonly IRepositories<int, User> _repoUser;

        private readonly IRepositories<int, UserRechargeHistory> _rechargeRepositories;

        private readonly IRepositories<int, UserTradingHistory> _tradingRepositories;

        private readonly IRepositories<int, PageGem> _pageGemRepositories;

        private readonly IRepositories<int, Champion> _champRepositories;

        private readonly IRepositories<int, Skin> _skinRepositories;

        public AccountRelationDataService(IRepositories<int, Rank> repoGank,
            IRepositories<int, Skin> repoSkin,
            IRepositories<int, Account> repoAccount,
            IRepositories<int, UserRechargeHistory> rechargeRepositories,
            IRepositories<int, UserTradingHistory> tradingRepositories,
            IRepositories<int, User> repoUser,
            IRepositories<int, PageGem> pageGemRepositories,
            IRepositories<int, Champion> champRepositories,
            IRepositories<int, Skin> skinRepositories)
        {
            _repoGank = repoGank;
            _repoSkin = repoSkin;
            _repoAccount = repoAccount;
            _rechargeRepositories = rechargeRepositories;
            _tradingRepositories = tradingRepositories;
            _repoUser = repoUser;
            _pageGemRepositories = pageGemRepositories;
            _champRepositories = champRepositories;
            _skinRepositories = skinRepositories;
        }

        public ChampionDto CreateChampion(ChampionDto champ)
        {

            var champTemp = Mapper.Map<Champion>(champ);
            var result = _champRepositories.Create(champTemp);
            if (_champRepositories.SaveChanges() > 0)
            {
                return Mapper.Map<ChampionDto>(result);
            }

            return null;
        }

        public SkinDto CreateSkin(SkinDto skin)
        {
            var skinTemp = Mapper.Map<Skin>(skin);
            var result = _skinRepositories.Create(skinTemp);
            if (_skinRepositories.SaveChanges() > 0)
            {
                return Mapper.Map<SkinDto>(result);
            }

            return null;
        }

        public IPagedList<ChampionDto> ReadChamp(int currentIndex, int pageSize, string searchString = "")
        {
            var query = _champRepositories.Read(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.ChampionName.Contains(searchString));
            }
            var result = query
                .OrderBy(a => a.ChampionName)
                .Select(a => new ChampionDto()
                {
                    CreatedDate = a.CreatedDate,
                    Id = a.Id,
                    IsDeleted = a.IsDeleted,
                    ChampionName = a.ChampionName,
                    Avatar = a.Avatar,
                    UpdatedDate = a.UpdatedDate
                }).ToPagedList(currentIndex, pageSize);

            return result;
        }

        public IList<ChampionDto> ReadChamp(string searchString = "")
        {
            var result = _champRepositories.Read(a => !a.IsDeleted);
            return Mapper.Map<IList<ChampionDto>>(result);
        }

        public int DeleteChamp(int champId)
        {
            var champ = _champRepositories.ReadOne(a => a.Id == champId);

            champ.IsDeleted = true;

            return _champRepositories.SaveChanges();
        }

        public IPagedList<SkinDto> ReadSkin(int currentIndex, int pageSize, string searchString = "")
        {
            var query = _skinRepositories.Read(a => !a.IsDeleted);
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.SkinName.Contains(searchString));
            }
            var result = query
                .OrderByDescending(a => a.SkinName)
                .ThenBy(a => a.SkinName)
                .Select(a => new SkinDto()
                {
                    CreatedDate = a.CreatedDate,
                    Id = a.Id,
                    IsDeleted = a.IsDeleted,
                    SkinName = a.SkinName,
                    Avatar = a.Avatar,
                    UpdatedDate = a.UpdatedDate
                }).ToPagedList(currentIndex, pageSize);

            return result;
        }

        public IList<SkinDto> ReadSkin(string searchString = "", bool isParentOnly = false)
        {
            IQueryable<Skin> result;
            result = _skinRepositories.Read(a => !a.IsDeleted);

            return Mapper.Map<IList<SkinDto>>(result);
        }

        public int DeleteSkin(int skinId)
        {
            var skin = _skinRepositories.ReadOne(a => a.Id == skinId);

            skin.IsDeleted = true;

            return _skinRepositories.SaveChanges();
        }

        public PageGemDto CreatePageGem(PageGemDto data)
        {
            var pg = Mapper.Map<PageGem>(data);

            _pageGemRepositories.Create(pg);

            if (_pageGemRepositories.SaveChanges() > 0)
            {
                return data;
            }
            return null;

        }

        public int DeletePageGemById(int pageGemId)
        {
            var imagesDeleted = _pageGemRepositories.ReadOne(a => a.Id == pageGemId);
            _pageGemRepositories.Delete(a => a.Id == pageGemId);
            int result = _pageGemRepositories.SaveChanges();

            if (result > 0)
            {
               FileUlti.DeleteFile(imagesDeleted.ImageUrl);
            }

            return result;
        }

        public int DeletePageGemByAccountId(int accountId)
        {
            var imagesDeleted = _pageGemRepositories.Read(a => a.AccountId == accountId).Select(a => a.ImageUrl);

            _pageGemRepositories.Delete(a => a.AccountId == accountId);
            int result = _pageGemRepositories.SaveChanges();

            if (result > 0)
            {
                foreach (var img in imagesDeleted)
                {
                    FileUlti.DeleteFile(img);
                }
            }

            return result;
        }

        public IPagedList<PageGemDto> ReadPageGem(int currentIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IList<RankDto> ReadRankForFilter()
        {
            return Mapper.Map<IList<RankDto>>(_repoGank.Read(a => !a.IsDeleted));
        }

        public IList<SkinDto> ReadSkinForFilter()
        {
            return Mapper.Map<IList<SkinDto>>(_repoSkin.Read(a => !a.IsDeleted));
        }

        public IList<string> ReadPriceRangeForFilter()
        {
            return new List<string>()
            {
                "10k",
                "20k",
                "30k",
                "40k",
                "50k",
                "50k - 500k",
                "0k - 10k",
                "VIP"
            };
        }

        public IList<AccountDto> ReadAccountBySamePrice(float price, IList<int> ignoreId = null)
        {
            var query = _repoAccount.Read(a => a.IsAvailable && (a.Price == price));

            if (ignoreId != null)
            {
                query = query.Where(a => !ignoreId.Contains(a.Id));
            }

            var result = query
                .Include(a => a.NumberOfPageGems)
                .Include(a => a.Skins)
                .Include(a => a.Champions)
                .Select(a => new AccountDto()
            {
                CreatedDate = a.CreatedDate,
                RankId = a.RankId,
                RankName = a.Rank.RankName,
                UserName = a.UserName,
                Avatar = a.Avatar,
                IsAvailable = a.IsAvailable,
                Description = a.Description,
                Id = a.Id,
                IsHot = a.IsHot,
                Password = a.Password,
                Price = a.Price,
                Title = a.Title,
                UpdatedDate = a.UpdatedDate,
                NumberOfPageGem = a.NumberOfPageGems.Count,
                NumberOfChamps = a.Champions.Count,
                NumberOfSkins = a.Skins.Count
            })
                .OrderBy(a => a.IsHot)
                .ThenByDescending(a => a.CreatedDate)
                .Take(4).ToList();

            return result;
        }

        public IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(string generalUserId, int currentIndex, int pageSize)
        {
            var result = 
                _rechargeRepositories.Read(a => a.User.GeneralUserId == generalUserId)
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedDate)
                .Select(a => new UserRechargeHistoryDto()
                    {
                        Id = a.Id,
                        UserId = a.UserId,
                        CreatedDate = a.CreatedDate,
                        SerialNumber = a.SerialNumber,
                        Message = a.Message,
                        ParValue = a.ParValue,
                        PinNumber = a.PinNumber,
                        UpdatedDate = a.UpdatedDate
                    })
                .ToPagedList(currentIndex, pageSize);

            return result;
        }

        public IPagedList<UserTradingHistoryDto> ReadTradingHistories(string generalUserId, int currentIndex, int pageSize)
        {
            var result = _tradingRepositories.Read(a => a.User.GeneralUserId == generalUserId)
                                            .Include(a => a.User)
                                            .Include(a => a.Account)
                                            .Include(a => a.Account.Rank)
                                            .OrderByDescending(a => a.CreatedDate)
                                            .Select(a => new UserTradingHistoryDto()
                                            {
                                                UserName = a.User.GeneralUser.UserName,
                                                PriceOfAccount = a.Account.Price,
                                                CreatedDate = a.CreatedDate,
                                                AccountId = a.AccountId,
                                                AccountName = a.Account.UserName,
                                                Password = a.Account.Password,
                                                RankName = a.Account.Rank.RankName,
                                                UserId = a.UserId
                                            }).ToPagedList(currentIndex, pageSize);
            return result;
        }

        public IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(int currentIndex, int pageSize, int month)
        {
            if (month == 0)
            {
                var result =
                    _rechargeRepositories.Read(a => true)
                        .Include(a => a.User)
                        .OrderByDescending(a => a.CreatedDate)
                        .ThenBy(a => a.UserId)
                        .Select(a => new UserRechargeHistoryDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            CreatedDate = a.CreatedDate,
                            SerialNumber = a.SerialNumber,
                            Message = a.Message,
                            ParValue = a.ParValue,
                            PinNumber = a.PinNumber,
                            UpdatedDate = a.UpdatedDate,
                            Email = a.User.GeneralUser.UserName,
                            CardType = a.CardType
                        })
                        .ToPagedList(currentIndex, pageSize);

                return result;
            }

            var resultRp = _repoUser.Read(a => a.IsActive && a.UserRechargeHistories.Any(b => b.CreatedDate.Month == month && b.CreatedDate.Year == DateTime.Now.Year))
                                    .Include(a => a.UserRechargeHistories)
                                    .OrderByDescending(a => a.UserRechargeHistories.Where(b => b.CreatedDate.Month == month && b.CreatedDate.Year == DateTime.Now.Year)
                                                                                    .Sum(b => b.ParValue))
                                    .Select(a => new UserRechargeHistoryDto()
                                    {
                                        UserId = a.Id,
                                        ParValue = a.UserRechargeHistories.Where(b => b.CreatedDate.Month == month && b.CreatedDate.Year == DateTime.Now.Year).Sum(b => b.ParValue) ,
                                        Email = a.GeneralUser.UserName,
                                        CreatedDate = a.UserRechargeHistories.OrderByDescending(b => b.CreatedDate).FirstOrDefault().CreatedDate
                                    })
                                    .ToPagedList(currentIndex, pageSize);

            return resultRp;
        }

        public IPagedList<UserTradingHistoryDto> ReadTradingHistories(int currentIndex, int pageSize)
        {
            var result = _tradingRepositories.Read(a => true)
                                .Include(a => a.User)
                                .Include(a => a.Account)
                                .Include(a => a.Account.Rank)
                                .OrderByDescending(a => a.CreatedDate)
                                .Select(a => new UserTradingHistoryDto()
                                {
                                    UserName = a.User.GeneralUser.UserName,
                                    PriceOfAccount = a.Account.Price,
                                    CreatedDate = a.CreatedDate,
                                    AccountId = a.AccountId,
                                    AccountName = a.Account.UserName,
                                    Password = a.Account.Password,
                                    RankName = a.Account.Rank.RankName,
                                    UserId = a.UserId
                                }).ToPagedList(currentIndex, pageSize);
            return result;
        }

        public int UploadDataFromJson(string json)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};

            Dictionary<string, object> data = (Dictionary<string, object>)json_serializer.DeserializeObject(json);

            var champs = JsonConvert.SerializeObject(data["data"]);

            var detailsInformation = JsonConvert.DeserializeObject<Dictionary<string, ChampUploadDto>>(champs);
         
            return ProccessingDataToDb(detailsInformation);
        }

        private int ProccessingDataToDb(Dictionary<string, ChampUploadDto> detailsInformation)
        {
            //TODO: Implement for case delete champ or skin if not existed in json file, upda skin problem

            foreach (KeyValuePair<string, ChampUploadDto> champ in detailsInformation)
            {
                Champion champData;
                if (_champRepositories.Read(a => a.Id == champ.Value.key).Any())
                {
                    champData = _champRepositories.ReadOne(a => a.Id == champ.Value.key);
                    champData.ChampionName = champ.Value.name;
                    champData.UpdatedDate = DateTime.Now;
                    champData.Avatar = $"../Images/champion/{champ.Key}.png";
                    champData.Skins = champ.Value.skins.Select(a =>
                    {
                        Skin skinData;
                        if (_repoSkin.Read(b => b.Id == a.id).Any())
                        {
                            skinData = _repoSkin.ReadOne(b => b.Id == a.id);
                            skinData.Avatar = $"../Images/skins/championsskin_{a.id}.jpg";
                            skinData.ChampionId = champ.Value.key;
                            skinData.UpdatedDate = DateTime.Now;
                            skinData.SkinName = a.name;
                        }
                        else
                        {
                            skinData= new Skin()
                            {
                                Avatar = $"../Images/skins/championsskin_{a.id}.jpg",
                                ChampionId = champ.Value.key,
                                CreatedDate = DateTime.Now,
                                SkinName = a.name,
                                Id = a.id
                            };
                        }

                        return skinData;
                    }).ToList();

                    _champRepositories.SaveChanges();
                }
                else
                {
                    champData = new Champion()
                    {
                        ChampionName = champ.Value.name,
                        CreatedDate = DateTime.Now,
                        Id = champ.Value.key,
                        Avatar = $"../Images/champion/{champ.Key}.png",
                        Skins = champ.Value.skins.Select(a => new Skin()
                        {
                            Avatar = $"../Images/skins/championsskin_{a.id}.jpg",
                            ChampionId = champ.Value.key,
                            CreatedDate = DateTime.Now,
                            SkinName = a.name,
                            Id = a.id
                        }).ToList()
                    };
                    _champRepositories.Create(champData);
                }

               
            }
            return _champRepositories.SaveChanges();
        }



        public IList<SkinDto> ReadSkinByAccount(int accountId)
        {
            var result = _repoSkin.Read(a => !a.IsDeleted && a.Accounts.Any(b => b.Id == accountId));
            return Mapper.Map<IList<SkinDto>>(result);
        }

        public IList<ChampionDto> ReadChampByAccount(int accountId)
        {
            var result = _champRepositories.Read(a => !a.IsDeleted && a.Accounts.Any(b => b.Id == accountId));
            return Mapper.Map<IList<ChampionDto>>(result);
        }

        public int AssignOrUnassignChamp(int accountId, string champs)
        {
            var chmps = champs.Split(',').Distinct();

            var account = _repoAccount.ReadOne(a => a.Id == accountId);
            var champ = _champRepositories.Read(a => !a.IsDeleted && chmps.Contains(a.ChampionName), true).ToList();

            account.Champions.Clear();
            account.Champions = champ;
           
            return _repoAccount.SaveChanges();
        }

        public int AssignOrUnassignSkin(int accountId, string skins)
        {
            var skns = skins.Split(',').Distinct();

            var account = _repoAccount.ReadOne(a => a.Id == accountId);
            var sks = _skinRepositories.Read(a => !a.IsDeleted && skns.Contains(a.SkinName), true).ToList();

            account.Skins.Clear();
            account.Skins = sks;

            return _repoAccount.SaveChanges();
        }
    }
}
