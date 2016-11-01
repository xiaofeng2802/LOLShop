using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Business.Contracts
{
    public interface IAccountService
    {

        AccountDto Create(AccountDto entity);

        IPagedList<AccountDto> Read(int currentIndex, int pageSize);

        IList<AccountDto> Read();

        AccountDto Update(AccountDto entity);

        int Delete(int entityId);

        IPagedList<AccountDto> FilterByGankPriceSkin(int currentIndex, int pageSize, int? gankFilter, string priceFilter, int? skinFilter);
    }
}
