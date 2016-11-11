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

        PageGemDto CreatePageGem(PageGemDto data);

        int DeletePageGemById(int pageGemId);

        int DeletePageGemByAccountId(int accountId);

        IPagedList<PageGemDto> ReadPageGem(int currentIndex, int pageSize);
        IList<RankDto> ReadRankForFilter();

        IList<SkinDto> ReadSkinForFilter();

        IList<string> ReadPriceRangeForFilter();

        IList<AccountDto> ReadAccountByPriceRange(decimal from, decimal to);

        IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(string generalUserId, int currentIndex, int pageSize);

        IPagedList<UserTradingHistoryDto> ReadTradingHistories(string generalUserId, int currentIndex, int pageSize);
    }
}
