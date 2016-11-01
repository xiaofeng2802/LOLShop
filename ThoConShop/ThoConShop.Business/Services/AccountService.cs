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

namespace ThoConShop.Business.Services
{
    public class AccountService : IAccountService
    {
        readonly IRepositories<int, Account> _repo;

        public AccountService(IRepositories<int, Account> repo)
        {
            _repo = repo;
        }

        public AccountDto Create(AccountDto entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public IPagedList<AccountDto> FilterByGankPriceSkin(int currentIndex, int pageSize, int? gankFilter, string priceFilter, int? skinFilter)
        {
            var result = _repo.Read(a => true);
            if (gankFilter != null)
            {
                result = result.Where(a => a.GankId == gankFilter);
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

            if (skinFilter != null)
            {
                result = result.Include(a => a.Skins).Where(a => a.Skins.Any(b => b.GroupId == skinFilter));
            }

            return result.Select(a => new AccountDto()
            {
                CreatedDate = a.CreatedDate,
                GankId = a.GankId,
                UserName = a.UserName,
                Avatar = a.Avatar,
                IsAvailable = a.IsAvailable,
                AccountName = a.AccountName,
                Description = a.Description,
                Id = a.Id,
                IsHot = a.IsHot,
                Password = a.Password,
                Price = a.Price,
                Title = a.Title,
                UpdatedDate = a.UpdatedDate
            }).OrderByDescending(a => a.IsHot).ThenByDescending(a => a.CreatedDate).ToPagedList(currentIndex, pageSize);
        }

        public IList<AccountDto> Read()
        {
            return Mapper.Map<IList<AccountDto>>(_repo.Read(a => true).ToList());
        }

        public IPagedList<AccountDto> Read(int currentIndex, int pageSize)
        {
            var result = _repo.Read(a => true)
                              .Select(a =>  new AccountDto()
                                {
                                    CreatedDate = a.CreatedDate,
                                    GankId = a.GankId,
                                    UserName = a.UserName,
                                    Avatar = a.Avatar,
                                    IsAvailable = a.IsAvailable,
                                    AccountName = a.AccountName,
                                    Description = a.Description,
                                    Id = a.Id,
                                    IsHot = a.IsHot,
                                    Password = a.Password,
                                    Price = a.Price,
                                    Title = a.Title,
                                    UpdatedDate = a.UpdatedDate
                                })
                              .OrderBy(a => a.GankId)
                              .ToPagedList(currentIndex, pageSize);

            return result;
        }

        public AccountDto Update(AccountDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
