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
using System.IO;

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

            var result = _repoAccount.Create(account);
            
            if (_repoAccount.SaveChanges() > 0)
            {
                var accountAdded = _repoAccount.ReadOne(a => a.Id == result.Id);

                accountAdded.Champions = ChampionForCreation(champ);
                accountAdded.Skins = SkinForCreation(skin);

                _repoAccount.SaveChanges();
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

                _repoChamp.SaveChanges();
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
            var avatar = _repoAccount.ReadOne(a => a.Id == entityId).Avatar;
            var data = _repoAccount.ReadOne(a => a.Id == entityId).NumberOfPageGems.Select(a => a.ImageUrl).ToList();
            _repoAccount.Delete(a => a.Id == entityId);
            var result = _repoAccount.SaveChanges();
            if (result > 0)
            {
                FileUlti.DeleteFile(avatar);

                foreach (var page in data)
                {
                    FileUlti.DeleteFile(page);
                }
            }
            return result;
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
        }

        public double GetPrice(int accountId)
        {
            var result = _repoAccount.ReadOne(a => a.Id == accountId);
            return (double) result.Price;
        }

        public IPagedList<AccountDto> FilterByRankPriceSkin(int currentIndex, int pageSize, int? rankFilter, string priceFilter, string skinFilter, string champFilter, string orderBy)
        {
            var query = _repoAccount.Read(a => a.IsAvailable);

            if (rankFilter != null && rankFilter > 0)
            {
                query = query.Where(a => a.RankId == rankFilter);
            }

            if (!string.IsNullOrEmpty(priceFilter))
            {
                if (priceFilter == "VIP")
                {
                    query = query.Where(a => a.Price >= 500000 && a.Price <= 1000000);
                }
                else
                {
                    char[] separators = new char[] { ' ', '<', '>' };

                    priceFilter = priceFilter.Replace(separators, string.Empty);
                    priceFilter = priceFilter.Replace("k", "000");

                    var prices = priceFilter.Split('-').Select(int.Parse).ToList();

                    switch (prices.Count)
                    {
                        case 1:
                            int price = prices[0];
                            query = query.Where(a => a.Price == price);
                            break;
                        case 2:
                            int price1 = prices[0], price2 = prices[1];
                            query = query.Where(a => a.Price >= price1 && a.Price <= price2);
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(skinFilter))
            {
                var id = _repoSkin.ReadOne(a => skinFilter == a.SkinName).Id;

                query = query.Include(a => a.Skins).Where(a => a.Skins.Any(b => b.Id == id));
            }

            if (!string.IsNullOrEmpty(champFilter))
            {
                var id = _repoChamp.ReadOne(a => a.ChampionName == champFilter).Id;
                query = query.Include(a => a.Champions).Where(a => a.Champions.Any(b => b.Id == id));
            }
            query = query.Include(a => a.Rank)
                .Include(a => a.NumberOfPageGems)
                .Include(a => a.Champions)
                .Include(a => a.Skins)
                .Where(a => a.IsAvailable);

            if (orderBy == "ChampOrder")
            {
                query = query.OrderByDescending(a => a.Champions.Count).ThenByDescending(a => a.CreatedDate);
            }
            else if(orderBy == "SkinOrder")
            {
                query = query.OrderByDescending(a => a.Skins.Count).ThenByDescending(a => a.CreatedDate);
            }
            else
            {
                query = query.OrderByDescending(a => a.CreatedDate);
            }
            var result = query.Select(a => new AccountDto()
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
                NumberOfSkins = a.Skins.Count,
                EventPrice = a.EventPrice
            });
            return result.ToPagedList(currentIndex, pageSize);
        }

        public IList<AccountDto> Read()
        {
            return Mapper.Map<IList<AccountDto>>(_repoAccount.Read(a => a.IsAvailable).ToList());
        }

        public IPagedList<AccountDto> Read(int currentIndex, int pageSize, bool isAvailableOnly = true, string searchString = "")
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
                    NumberOfSkins = a.Skins.Count,
                    EventPrice = a.EventPrice
                });
                             
            if (!string.IsNullOrEmpty(searchString))
            {
                return result.Where(a => a.Id.ToString().Contains(searchString)).ToList().OrderByDescending(a => a.CreatedDate)
                              .ToPagedList(currentIndex, pageSize);
            }

            return result.OrderByDescending(a => a.CreatedDate)
                              .ToPagedList(currentIndex, pageSize);
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
            data.EventPrice = entity.EventPrice;


            if (!string.IsNullOrEmpty(champ))
            {
                data.Champions.Clear();
                _repoAccount.SaveChanges();
                data.Champions = ChampionForCreation(champ);
                _repoAccount.SaveChanges();
            }

            if (!string.IsNullOrEmpty(skin))
            {
                data.Skins.Clear();
                _repoAccount.SaveChanges();
                data.Skins = SkinForCreation(skin);
                _repoAccount.SaveChanges();
            }

            if (_repoAccount.SaveChanges() > 0)
            {
                return entity;
            }
            return null;
        }
    }
}
