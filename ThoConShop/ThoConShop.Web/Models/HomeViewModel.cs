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

        public IList<GankDto> GanksFilter { get; set; }

        public IList<SkinDto> SkinsFilter { get; set; }

        public IList<string> PriceFilter { get; set; }

        public int? CurrentGankFilter { get; set; }

        public int? CurrentSkinFilter { get; set; }

        public string CurrentPriceFilter { get; set; }
    }
}