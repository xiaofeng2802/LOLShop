﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Business.Contracts
{
    public interface IHistoryService
    {
        UserRechargeHistoryDto Create(UserRechargeHistoryDto rechargeHistory);
        UserTradingHistoryDto Create(UserTradingHistoryDto tradingHistory);
    }
}
