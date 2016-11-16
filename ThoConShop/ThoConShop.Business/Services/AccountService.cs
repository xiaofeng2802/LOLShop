using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;
using PagedList;
using ThoConShop.DataSeedWork;
using ThoConShop.DataSeedWork.Extensions;
using ThoConShop.DataSeedWork.Ulti;

namespace ThoConShop.Business.Services
{
    public class AccountService : IAccountService
    {
        readonly IRepositories<int, Account> _repoAccount;
        readonly IRepositories<int, Rank> _repoRank;
        readonly IRepositories<int, Skin> _repoSkin;
        readonly IRepositories<int, Champion> _repoChamp;

        public AccountService(IRepositories<int, Account> repo,
            IRepositories<int, Rank> repoRank,
             IRepositories<int, Skin> repoSkin,
             IRepositories<int, Champion> repoChamp)
        {
            _repoAccount = repo;
            _repoRank = repoRank;
            _repoSkin = repoSkin;
            _repoChamp = repoChamp;
        }

        public AccountDto Create(AccountDto entity, string champ, string skin)
        {
            var account = Mapper.Map<Account>(entity);

            account.Champions = ChampionForCreation(champ);
            account.Skins = SkinForCreation(skin);

            var result = _repoAccount.Create(account);
            
            if (_repoAccount.SaveChanges() > 0)
            {
                return Mapper.Map<AccountDto>(result);
            }

            return null;
        }

        private IList<Champion> ChampionForCreation(string data)
        {
            List<Champion> champs = new List<Champion>();
            if (!string.IsNullOrEmpty(data))
            {
                var splitData = data.Split(new[] { '\r', '\n' }).Where(a => !string.IsNullOrEmpty(a));


                foreach (var item in splitData)
                {
                    champs.Add(_repoChamp.ReadOne(a => a.ChampionName == item));
                }
            }
           

            return champs;
        }

        private IList<Skin> SkinForCreation(string data)
        {
            List<Skin> skins = new List<Skin>();

            if (!string.IsNullOrEmpty(data))
            {
                var splitData = data.Split(new[] { '\r', '\n' }).Where(a => !string.IsNullOrEmpty(a));


                foreach (var item in splitData)
                {

                    var firstIndex = item.IndexOf("_", StringComparison.Ordinal);
                    var end = item.LastIndexOf(".", StringComparison.Ordinal);
                    var stringData = item.Substring((firstIndex + 1), ((end - firstIndex) - 1));
                    int key = 0;

                    if (int.TryParse(stringData, out key))
                    {
                        skins.Add(_repoSkin.ReadOne(a => a.Id == key));
                    }
                }
            }

            return skins;
        }

        public int Delete(int entityId)
        {
            _repoAccount.Delete(a => a.Id == entityId);
            return _repoAccount.SaveChanges();
        }

        public AccountDto Edit(int accountId)
        {
            if (accountId > 0)
            {
                var accountDto = Mapper.Map<AccountDto>(_repoAccount.Read(a => true)
                                                              .Include(a => a.Champions)
                                                              .Include(a => a.Skins)
                                                              .Include(a => a.NumberOfPageGems)
                                                              .SingleOrDefault(a => a.Id == accountId));
                return accountDto;
            }
            return null;
        }

        public AccountDto ReadOneById(int accountId)
        {
            var result = _repoAccount.ReadOne(a => a.Id == accountId);

            return Mapper.Map<AccountDto>(result);
        }

        public void SoldAccount(int accountId)
        {
            var acc = _repoAccount.ReadOne(a => a.Id == accountId);
            acc.IsAvailable = false;
             _repoAccount.Update(acc);
            FileUlti.DeleteFile(acc.Avatar);

            foreach (var page in acc.NumberOfPageGems)
            {
                FileUlti.DeleteFile(page.ImageUrl);
            }

            _repoAccount.SaveChanges();

        }

        public double GetPrice(int accountId)
        {
            var result = _repoAccount.ReadOne(a => a.Id == accountId);
            return (double) result.Price;
        }

        public IPagedList<AccountDto> FilterByRankPriceSkin(int currentIndex, int pageSize, int? gankFilter, string priceFilter, int? skinFilter)
        {
            var result = _repoAccount.Read(a => a.IsAvailable);

            if (gankFilter != null && gankFilter > 0)
            {
                var idList = _repoRank.ReadOne(a => a.Id == gankFilter && !a.IsDeleted).Children.Select(a => a.Id).ToList();
              
                idList.Add(gankFilter ?? 0);
                
                result = result.Where(a => idList.Any(b => b == a.RankId));
            }

            if (!string.IsNullOrEmpty(priceFilter))
            {
                bool isSmaller = priceFilter.Contains("<");
                char[] separators = new char[] { ' ','<', '>' };

                priceFilter = priceFilter.Replace(separators, string.Empty);
                priceFilter = priceFilter.Replace("k", "000");

                var prices = priceFilter.Split('-').Select(int.Parse).ToList();

                switch (prices.Count)
                {
                    case 1:
                        int price = prices[0];
                        result = isSmaller
                            ? result.Where(a => a.Price < price)
                            : result.Where(a => a.Price > price);
                        break;
                    case 2:
                        int price1 = prices[0], price2 = prices[1];
                        result = result.Where(a => a.Price >= price1 && a.Price <= price2);
                        break;
                }
            }

            if (skinFilter != null && skinFilter > 0)
            {
                var idList = _repoSkin.ReadOne(a => a.Id == skinFilter).Children.Select(a => a.Id).ToList();

                idList.Add(skinFilter ?? 0);

                result = result.Include(a => a.Skins).Where(a => a.Skins.Any(b => idList.Contains(b.Id)));
            }

            return result.Include(a => a.Rank).Where(a => a.IsAvailable).Select(a => new AccountDto()
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
            }).OrderByDescending(a => a.IsHot)
                .ThenByDescending(a => a.CreatedDate)
                .ToPagedList(currentIndex, pageSize);
        }

        public IList<AccountDto> Read()
        {
            return Mapper.Map<IList<AccountDto>>(_repoAccount.Read(a => a.IsAvailable).ToList());
        }

        public IPagedList<AccountDto> Read(int currentIndex, int pageSize, bool isAvailableOnly = true)
        {
            var query = _repoAccount.Read(a => true);

            if (isAvailableOnly)
            {
                query = query.Where(a => a.IsAvailable == isAvailableOnly);
            }
            var result = query.Include(a => a.Rank)
                              .Include(a => a.NumberOfPageGems)
                              .Include(a => a.Champions)
                              .Include(a => a.Skins)
                              .Select(a =>  new AccountDto()
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
                              .OrderByDescending(a => a.CreatedDate)
                              .ToPagedList(currentIndex, pageSize);

            return result;
        }

        public AccountDto Update(AccountDto entity, string champ, string skin)
        {
            var data = _repoAccount.ReadOne(a => a.Id == entity.Id);
            data.Description = entity.Description;
            data.Password = entity.Password ?? data.Password;
            data.UpdatedDate = DateTime.Now;
            data.Price = entity.Price;
            data.RankId = entity.RankId;
            data.UserName = entity.UserName;


            if (!string.IsNullOrEmpty(champ))
            {
                data.Champions = new List<Champion>();
                _repoAccount.SaveChanges();
                data.Champions = ChampionForCreation(champ);
            }

            if (!string.IsNullOrEmpty(skin))
            {
                data.Skins= new List<Skin>();
                _repoAccount.SaveChanges();
                data.Skins = SkinForCreation(skin);
            }

            if (_repoAccount.SaveChanges() > 0)
            {
                return entity;
            }
            return null;
        }
    }
}
