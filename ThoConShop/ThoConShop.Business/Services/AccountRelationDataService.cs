using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public AccountRelationDataService(IRepositories<int, Rank> repoGank,
            IRepositories<int, Skin> repoSkin,
            IRepositories<int, Account> repoAccount,
            IRepositories<int, UserRechargeHistory> rechargeRepositories,
            IRepositories<int, UserTradingHistory> tradingRepositories,
            IRepositories<int, User> repoUser,
            IRepositories<int, PageGem> pageGemRepositories)
        {
            _repoGank = repoGank;
            _repoSkin = repoSkin;
            _repoAccount = repoAccount;
            _rechargeRepositories = rechargeRepositories;
            _tradingRepositories = tradingRepositories;
            _repoUser = repoUser;
            _pageGemRepositories = pageGemRepositories;
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
            return Mapper.Map<IList<RankDto>>(_repoGank.Read(a => a.GroupId == null));
        }

        public IList<SkinDto> ReadSkinForFilter()
        {
            return Mapper.Map<IList<SkinDto>>(_repoSkin.Read(a => a.GroupId == null));
        }

        public IList<string> ReadPriceRangeForFilter()
        {
            return new List<string>()
            {
                "> 1000k",
                "500k - 1000k",
                "200k - 500k",
                "100k - 200k",
                "< 100k"
            };
        }

        public IList<AccountDto> ReadAccountByPriceRange(decimal from, decimal to)
        {
            var result = _repoAccount.Read(a => a.IsAvailable && (a.Price >= from && a.Price <= to))
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
                                           UpdatedDate = a.UpdatedDate
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
                                                Password = a.Account.Password,
                                                RankName = a.Account.Rank.RankName,
                                                UserId = a.UserId
                                            }).ToPagedList(currentIndex, pageSize);
            return result;
        }
    }
}
