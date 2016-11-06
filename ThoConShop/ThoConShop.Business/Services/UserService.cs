using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<int, User> _userRepositories;

        public UserService(IRepositories<int, User> userRepositories)
        {
            _userRepositories = userRepositories;
        }

        public UserDto Create(UserDto entity)
        {
            var user = Mapper.Map<User>(entity);
            _userRepositories.Create(user);
            _userRepositories.SaveChanges();
            return entity;
        }

        public IPagedList<UserDto> Read(int currentIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IList<UserDto> Read()
        {
            throw new NotImplementedException();
        }

        public UserDto Update(UserDto entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public UserDto Edit(int accountId)
        {
            throw new NotImplementedException();
        }

        public UserDto ReadByGeneralUserId(string generalUserId)
        {
            var result = Mapper.Map<UserDto>(_userRepositories.ReadOne(a => a.GeneralUserId == generalUserId));
            return result;
        }
    }
}
