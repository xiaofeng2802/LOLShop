using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.Identity;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Contracts
{
    public interface IUserService
    {
        UserDto Create(UserDto entity);

        IPagedList<UserDto> Read(int currentIndex, int pageSize, string searchString = "");

        IList<UserDto> Read();

        UserDto Update(UserDto entity);

        int Delete(int entityId);

        int DeactiveOrActive(int entityId);

        UserDto Edit(int accountId);

        UserDto ReadByGeneralUserId(string generalUserId);

        IPagedList<LuckyWheelItemDto> ReadLuckyWheelItem(int currentIndex, int pageSize);

        IList<LuckyWheelItemDto> ReadAllLuckyWheelItem();

        LuckyWheelItemDto CreateLuckyItem(LuckyWheelItemDto data);

        int DeleteLuckyItem(int id);

        IPagedList<LuckyWheelHistoryDto> ReadLuckyWheelHistory(int currentIndex, int pageSize);

        LuckyWheelHistoryDto CreateLuckyHistory(LuckyWheelHistoryDto data);

        int DeleteLuckyHistory(int id);

        LuckyWheelItemDto RandomWheelItem(out int resultForWheel);
    }
}
