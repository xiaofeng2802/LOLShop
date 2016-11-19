using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.Extensions;
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
            var rank = Mapper.Map<Rank>(data);

            var result = _repoRank.Create(rank);

            if (_repoRank.SaveChanges() > 0)
            {
                return Mapper.Map<RankDto>(result);
            }
            return null;
        }

        public IPagedList<RankDto> Read(int currentPage, int pageSize, bool isParentOnly = false, string searchString = "")
        {
            var data = _repoRank.Read(a => !a.IsDeleted);

            //if (isParentOnly)
            //{
            //    data = data.Where(a => a.GroupId == null);
            //}
            //else
            //{
            //    data = data.Where(a => a.GroupId != null);
            //}

            if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(a => a.RankName.Contains(searchString));
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

        public IList<RankDto> Read(bool? isParentOnly = false)
        {
            var result = _repoRank.Read(a => !a.IsDeleted);
          
            return Mapper.Map<IList<RankDto>>(result.OrderBy(a => a.RankName));
        }

        public int Updated(RankDto data)
        {
            throw new NotImplementedException();
        }

        public int Delete(int rankId)
        {
            _repoRank.Delete(a => a.Id == rankId);
            return _repoRank.SaveChanges();
        }
    }
}
