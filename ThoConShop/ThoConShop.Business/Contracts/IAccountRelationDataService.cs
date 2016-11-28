using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Contracts
{
    public interface IAccountRelationDataService
    {

        int UploadDataFromJson(string json);
        IList<SkinDto> ReadSkinByAccount(int accountId);

        IList<ChampionDto> ReadChampByAccount(int accountId);

        int AssignOrUnassignChamp(int accountId, string champs);
        int AssignOrUnassignSkin(int accountId, string skins);

        Dtos.ChampionDto CreateChampion(ChampionDto champ);
        Dtos.SkinDto CreateSkin(SkinDto skin);

        IPagedList<ChampionDto> ReadChamp(int currentIndex, int pageSize, string searchString = "");

        IList<ChampionDto> ReadChamp(string searchString = "");

        int DeleteChamp(int champId);

        IPagedList<SkinDto> ReadSkin(int currentIndex, int pageSize, string searchString = "");

        IList<SkinDto> ReadSkin(string searchString = "", bool isParentOnly = false);

        int DeleteSkin(int skinId);

        PageGemDto CreatePageGem(PageGemDto data);

        int DeletePageGemById(int pageGemId);

        int DeletePageGemByAccountId(int accountId);

        IPagedList<PageGemDto> ReadPageGem(int currentIndex, int pageSize);
        IList<RankDto> ReadRankForFilter();

        IList<SkinDto> ReadSkinForFilter();

        IList<string> ReadPriceRangeForFilter();

        IList<AccountDto> ReadAccountBySamePrice(float price, IList<int> ignoreId = null);

        IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(string generalUserId, int currentIndex, int pageSize);

        IPagedList<UserTradingHistoryDto> ReadTradingHistories(string generalUserId, int currentIndex, int pageSize);

        IPagedList<UserRechargeHistoryDto> ReadRechargeHistories(int currentIndex, int pageSize, int month);

        IPagedList<UserTradingHistoryDto> ReadTradingHistories(int currentIndex, int pageSize);
    }
}
