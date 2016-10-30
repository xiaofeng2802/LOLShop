using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Business.Contracts
{
    public interface IAccountService
    {

        AccountDto Create(AccountDto entity);

        IList<AccountDto> Read();

        AccountDto Update(AccountDto entity);

        int Delete(int entityId);
    }
}
