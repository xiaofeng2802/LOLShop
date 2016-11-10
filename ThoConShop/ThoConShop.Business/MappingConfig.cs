using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using AutoMapper;
using ThoConShop.DAL.Entities;
using ThoConShop.Business.Dtos;
using PagedList;

namespace ThoConShop.Business
{
    public class MappingConfig
    {
        public static void RegisteMapper()
        {
            Mapper.Initialize(a =>
            {
                a.CreateMap<Account, AccountDto>().ReverseMap();
                a.CreateMap<Rank, RankDto>().ReverseMap();
                a.CreateMap<Skin, SkinDto>().ReverseMap();
                a.CreateMap<Champion, ChampionDto>().ReverseMap();
                a.CreateMap<User, UserDto>().ReverseMap();
                a.CreateMap<UserRechargeHistoryDto, UserRechargeHistory>().ReverseMap();
                a.CreateMap<UserTradingHistory, UserTradingHistoryDto>().ReverseMap();
                a.CreateMap<PageGem, PageGemDto>().ReverseMap();
            }); 
        }
    }
}
