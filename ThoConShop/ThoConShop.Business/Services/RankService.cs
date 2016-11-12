using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class RankService : IRankService
    {
        private readonly IRepositories<int, Rank> _repoRank;

        public RankService(IRepositories<int, Rank> repoRank)
        {
            _repoRank = repoRank;
        }

        public RankDto Create(RankDto data)
        {
            throw new NotImplementedException();
        }

        public IPagedList<RankDto> Read(int currentPage, int pageSize, bool isParentOnly = false)
        {
            var data = _repoRank.Read(a => true);
            if (isParentOnly)
            {
                data = data.Where(a => a.GroupId == null);
            }
            else
            {
                data = data.Where(a => a.GroupId != null);
            }

            IPagedList<RankDto> result = data.OrderBy(a => a.RankName).Select(a => new RankDto()
            {
                Id = a.Id,
                CreatedDate = a.CreatedDate,
                RankName = a.RankName,
                RankImage = a.RankImage,
                UpdatedDate = a.UpdatedDate
            }).ToPagedList(currentPage, pageSize);

            return result;
        }

        public IList<RankDto> Read(bool isParentOnly = false)
        {
            var result = _repoRank.Read(a => true);
            if (isParentOnly)
            {
                result = result.Where(a => a.GroupId == null);
            }
            else
            {
                result = result.Where(a => a.GroupId != null);
            }

            return Mapper.Map<IList<RankDto>>(result.OrderByDescending(a => a.CreatedDate));
        }

        public int Updated(RankDto data)
        {
            throw new NotImplementedException();
        }

        public int Delete(int rankId)
        {
            throw new NotImplementedException();
        }
    }
}
