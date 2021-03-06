﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class UserRechargeHistoryDto : BaseDto<int>
    {
        public int? UserId { get; set; }

        public string Email { get; set; }

        public decimal ParValue { get; set; }

        public string SerialNumber { get; set; }
 
        public string PinNumber { get; set; }

        public string CardType { get; set; }

        public string Message { get; set; }
    }

    public class UserRechargeHistoryManagementDto
    {
        public string Email { get; set; }

        public double SumOfValue { get; set; }
    }
}
