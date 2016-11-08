using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IRepositories<int, UserRechargeHistory> _rechargeRepositories;
        private readonly IRepositories<int, UserTradingHistory> _tradingHistory;

        public HistoryService(IRepositories<int, UserRechargeHistory> rechargeRepositories,
            IRepositories<int, UserTradingHistory> tradingHistory)
        {
            _rechargeRepositories = rechargeRepositories;
            _tradingHistory = tradingHistory; 
        }

        public UserTradingHistoryDto Create(UserTradingHistoryDto tradingHistory)
        {
            var data = Mapper.Map<UserTradingHistory>(tradingHistory);

            _tradingHistory.Create(data);

            if (_tradingHistory.SaveChanges() > 0)
            {
                return tradingHistory;
            }
            return null;
        }

        public UserRechargeHistoryDto Create(UserRechargeHistoryDto rechargeHistory)
        {
            var data = Mapper.Map<UserRechargeHistory>(rechargeHistory);

            _rechargeRepositories.Create(data);
            if (_rechargeRepositories.SaveChanges() > 0)
            {
                return rechargeHistory;
            }

            return null;
        }
    }
}
