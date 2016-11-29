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

        public IList<string> PriceFilter { get; set; }

        public Dictionary<string, string> OrderByList
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                result.Add("ChampOrder", "Tướng Nhiều Nhất");
                result.Add("SkinOrder", "Trang Phục Nhiều Nhất");
                return result;
            }
        }

        public int? CurrentRankFilter { get; set; }

        public string CurrentSkinFilter { get; set; }

        public string CurrentPriceFilter { get; set; }

        public string CurrentChampFilter { get; set; }

        public string CurrentOrderBy { get; set; }

        public string News { get; set; }
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