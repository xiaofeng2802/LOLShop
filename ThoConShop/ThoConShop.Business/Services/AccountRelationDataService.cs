using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class AccountRelationDataService : IAccountRelationDataService
    {
        readonly IRepositories<int, Rank> _repoGank;

        readonly IRepositories<int, Skin> _repoSkin;

        readonly IRepositories<int, Account> _repoAccount;

        public AccountRelationDataService(IRepositories<int, Rank> repoGank,
            IRepositories<int, Skin> repoSkin,
            IRepositories<int, Account> repoAccount)
        {
            _repoGank = repoGank;
            _repoSkin = repoSkin;
            _repoAccount = repoAccount;
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
                                     .OrderBy(a => a.IsHot)
                                     .ThenByDescending(a => a.CreatedDate)
                                     .Take(4);

            return Mapper.Map<IList<AccountDto>>(result);
        }
    }
}
