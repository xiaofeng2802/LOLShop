using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Business.Contracts
{
    public interface IUserService
    {
        UserDto Create(UserDto entity);

        IPagedList<UserDto> Read(int currentIndex, int pageSize);

        IList<UserDto> Read();

        UserDto Update(UserDto entity);

        int Delete(int entityId);

        UserDto Edit(int accountId);

        UserDto ReadByGeneralUserId(string generalUserId);
    }
}
