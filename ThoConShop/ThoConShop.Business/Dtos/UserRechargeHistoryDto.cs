using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class UserRechargeHistoryDto : BaseDto<int>
    {
        public int? UserId { get; set; }
        
        public decimal ParValue { get; set; }

        public string SerialNumber { get; set; }
 
        public string PinNumber { get; set; }

        public string Message { get; set; }
    }
}
