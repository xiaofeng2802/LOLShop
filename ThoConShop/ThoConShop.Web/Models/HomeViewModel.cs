using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Web.Models
{
    public class AccountIndexViewModel
    {
        public PagedList.IPagedList<AccountDto> DataSource { get; set; }

        public IList<RankDto> RanksFilter { get; set; }

        public IList<SkinDto> SkinsFilter { get; set; }

        public IList<string> PriceFilter { get; set; }

        public int? CurrentRankFilter { get; set; }

        public int? CurrentSkinFilter { get; set; }

        public string CurrentPriceFilter { get; set; }
    }

    public class AccountEditViewModel
    {
        public AccountEditViewModel()
        {
            SuggestionAccounts = new List<AccountDto>();
        }
        public AccountDto CurrentAccount { get; set; }

        public IList<AccountDto> SuggestionAccounts { get; set; }
    }
}