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
    public class RechargeHistoryService : IRechargeHistoryService
    {
        private readonly IRepositories<int, UserRechargeHistory> _rechargeRepositories;

        public RechargeHistoryService(IRepositories<int, UserRechargeHistory> rechargeRepositories)
        {
            _rechargeRepositories = rechargeRepositories;
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
