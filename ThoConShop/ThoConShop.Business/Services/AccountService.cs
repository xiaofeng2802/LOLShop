using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class AccountService : IAccountService
    {
        readonly IRepositories<int, Account> repo;

        public AccountService(IRepositories<int, Account> _repo)
        {
            repo = _repo;
        }

        public AccountDto Create(AccountDto entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public IList<AccountDto> Read()
        {
            return Mapper.Map<IList<AccountDto>>(repo.Read(a => true).ToList());
        }

        public AccountDto Update(AccountDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
