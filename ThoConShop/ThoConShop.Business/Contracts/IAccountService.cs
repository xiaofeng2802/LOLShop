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

        AccountDto Create(AccountDto entity, string champ, string skin);

        IPagedList<AccountDto> Read(int currentIndex, int pageSize, bool isAvailableOnly = true);

        IList<AccountDto> Read();

        AccountDto Update(AccountDto entity, string champ, string skin);

        int Delete(int entityId);

        IPagedList<AccountDto> FilterByRankPriceSkin(int currentIndex, int pageSize, int? gankFilter, string priceFilter, string skinFilter);

        AccountDto Edit(int accountId);

        AccountDto ReadOneById(int accountId);

        void SoldAccount(int accountId);

        double GetPrice(int accountId);
    }
}
