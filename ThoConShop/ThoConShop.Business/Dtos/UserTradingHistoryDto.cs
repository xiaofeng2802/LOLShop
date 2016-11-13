using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class UserTradingHistoryDto : BaseDto<int>
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public string UserName { get; set; }

        public string RankName { get; set; }

        public string Password { get; set; }

        public float PriceOfAccount { get; set; }

        public string AccountName { get; set; }
    }
}
