using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Business.Contracts
{
    public interface IRankService
    {
        RankDto Create(RankDto data);

        IPagedList<RankDto> Read(int currentPage, int pageSize, bool isParentOnly = false, string searchString = "");

        IList<RankDto> Read(bool? isParentOnly = false);

        int Updated(RankDto data);

        int Delete(int rankId);
    }
}
