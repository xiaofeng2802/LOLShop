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
        readonly IRepositories<int, Gank> _repoGank;

        readonly IRepositories<int, Skin> _repoSkin;

        readonly IRepositories<int, Account> _repoAccount;

        public AccountRelationDataService(IRepositories<int, Gank> repoGank,
            IRepositories<int, Skin> repoSkin,
            IRepositories<int, Account> repoAccount)
        {
            _repoGank = repoGank;
            _repoSkin = repoSkin;
            _repoAccount = repoAccount;
        }

        public IList<GankDto> ReadGankForFilter()
        {
            return Mapper.Map<IList<GankDto>>(_repoGank.Read(a => true));
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
    }
}
