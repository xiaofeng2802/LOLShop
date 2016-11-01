using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }); 
        }
    }
}
