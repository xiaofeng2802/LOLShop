using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PagedList;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Dtos;
using ThoConShop.DataSeedWork.Identity;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<int, User> _userRepositories;

        ShopThoCon db = new ShopThoCon();

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
            var result = _userRepositories.Read(a => true)
                .Include(a => a.GeneralUser)
                .Select(a => new UserDto()
            {
                Email = a.GeneralUser.Email,
                Id = a.Id,
                Balance = a.Balance,
                CreatedDate = a.CreatedDate,
                GeneralUserId = a.GeneralUserId,
                IsActive = a.IsActive,
                UserName = a.GeneralUser.UserName
            }).OrderByDescending(a => a.CreatedDate)
            .ToPagedList(currentIndex, pageSize);

            return result;
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
            var userGenericId = _userRepositories.ReadOne(a => a.Id == entityId).GeneralUserId;
             _userRepositories.Delete(a => a.Id == entityId);
            if (_userRepositories.SaveChanges() > 0)
            {

                var user = db.Set<ApplicationUser>().SingleOrDefault(a => a.Id == userGenericId);
                db.Set<ApplicationUser>().Remove(user);
               
            }
            return db.SaveChanges();
        }

        public int DeactiveOrActive(int entityId)
        {
            var data = _userRepositories.ReadOne(a => a.Id == entityId);
            data.IsActive = !data.IsActive;

            return _userRepositories.SaveChanges();
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
