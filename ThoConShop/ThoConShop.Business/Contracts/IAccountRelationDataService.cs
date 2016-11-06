using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Contracts
{
    public interface IAccountRelationDataService
    {
        IList<RankDto> ReadRankForFilter();

        IList<SkinDto> ReadSkinForFilter();

        IList<string> ReadPriceRangeForFilter();

        IList<AccountDto> ReadAccountByPriceRange(decimal from, decimal to);

        IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(string generalUserId, int currentIndex, int pageSize);

        IPagedList<UserTradingHistoryDto> ReadtrTradingHistories(string generalUserId, int currentIndex, int pageSize);
    }
}
