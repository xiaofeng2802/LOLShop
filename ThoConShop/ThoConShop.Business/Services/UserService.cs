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
using ThoConShop.DataSeedWork.LuckyWheel;
using ThoConShop.DataSeedWork.Ulti;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositories<int, User> _userRepositories;

        private readonly IRepositories<int, LuckyWheelItem> _luckyWheelItemRepositories;
        private readonly IRepositories<int, LuckyWheelHistory> _luckyWheelHistoryRepositories;

        ShopThoCon db = new ShopThoCon();

        public UserService(IRepositories<int, User> userRepositories,
            IRepositories<int, LuckyWheelItem> luckyWheelItemRepositories,
            IRepositories<int, LuckyWheelHistory> luckyWheelHistoryRepositories)
        {
            _userRepositories = userRepositories;
            _luckyWheelItemRepositories = luckyWheelItemRepositories;
            _luckyWheelHistoryRepositories = luckyWheelHistoryRepositories;
        }

        public void UpdateBalanceUser(string generalUserId, float? balance)
        {
            var user = _userRepositories.ReadOne(a => a.GeneralUserId == generalUserId);
            if (user != null && balance != null)
            {
                user.Balance = balance ?? 0;
                _userRepositories.SaveChanges();
            }
        }

        public void UpdatePointUser(string generalUserId, int? point)
        {
            var user = _userRepositories.ReadOne(a => a.GeneralUserId == generalUserId);
            if (user != null && point != null)
            {
                user.Point = point ?? 0;
                _userRepositories.SaveChanges();
            }
        }

        public UserDto Create(UserDto entity)
        {
            var user = Mapper.Map<User>(entity);
            _userRepositories.Create(user);
            _userRepositories.SaveChanges();
            return entity;
        }

        public IPagedList<UserDto> Read(int currentIndex, int pageSize, string searchString = "")
        {
            var query = _userRepositories.Read(a => true);
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.GeneralUser.UserName.Contains(searchString));
            }
            var result = query
                .Include(a => a.GeneralUser)
                .Select(a => new UserDto()
            {
                Email = a.GeneralUser.Email,
                Id = a.Id,
                Balance = a.Balance,
                CreatedDate = a.CreatedDate,
                GeneralUserId = a.GeneralUserId,
                IsActive = a.IsActive,
                UserName = a.GeneralUser.UserName,
                Point = a.Point
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

        public IPagedList<LuckyWheelItemDto> ReadLuckyWheelItem(int currentIndex, int pageSize)
        {
            var result = _luckyWheelItemRepositories.Read(a => true)
                .OrderByDescending(a => a.CreatedDate)
                .ThenByDescending(a => a.DisplayName)
                .Select(a => new LuckyWheelItemDto()
                {
                    CreatedDate = a.CreatedDate,
                    ImageUrl = a.ImageUrl,
                    Id = a.Id,
                    Description = a.Description,
                    UpdatedDate = a.UpdatedDate,
                    DisplayName = a.DisplayName,
                    WinningPercent = a.WinningPercent,
                    IsUnlucky = a.IsUnlucky
                }).ToPagedList(currentIndex, pageSize);

            return result;
        }

        public IList<LuckyWheelItemDto> ReadAllLuckyWheelItem()
        {
            var result = _luckyWheelItemRepositories.Read(a => true)
                .OrderBy(a => a.DisplayName);

            return Mapper.Map<IList<LuckyWheelItemDto>>(result);
        }

        public LuckyWheelItemDto CreateLuckyItem(LuckyWheelItemDto data)
        {
            var entity = Mapper.Map<LuckyWheelItem>(data);
            entity.CreatedDate = DateTime.Now;
            entity.ImageUrl = "../Images/LuckyItem/" + FileUlti.SaveFile(data.FileImage, data.ImageUrl);

            var result = _luckyWheelItemRepositories.Create(entity);

            if (_luckyWheelItemRepositories.SaveChanges() > 0)
            {
                return Mapper.Map<LuckyWheelItemDto>(result);
            }

            return null;
        }

        public int DeleteLuckyItem(int id)
        {
            var img = _luckyWheelItemRepositories.ReadOne(a => a.Id == id).ImageUrl;
            if (!string.IsNullOrEmpty(img))
            {
                FileUlti.DeleteFile(img);
            }

            _luckyWheelItemRepositories.Delete(a => a.Id == id);
            var result = _luckyWheelItemRepositories.SaveChanges();
          
            return result;
        }

        public IPagedList<LuckyWheelHistoryDto> ReadLuckyWheelHistory(int currentIndex, int pageSize)
        {
            var result = _luckyWheelHistoryRepositories.Read(a => true)
                .OrderByDescending(a => a.CreatedDate)
                .Include(a => a.User)
                .Include(a => a.User.GeneralUser)
                .Select(a => new LuckyWheelHistoryDto()
                {
                  Id = a.Id,
                  CreatedDate = a.CreatedDate,
                  UpdatedDate = a.UpdatedDate,
                  NameDisplay = a.User.NameDisplay,
                  Result = a.Result
                }).ToPagedList(currentIndex, pageSize);

            return result;
        }

        public LuckyWheelHistoryDto CreateLuckyHistory(LuckyWheelHistoryDto data)
        {
            var entity = Mapper.Map<LuckyWheelHistory>(data);
            entity.CreatedDate = DateTime.Now;
            entity.UserId = data.UserId;

            var result = _luckyWheelHistoryRepositories.Create(entity);

            if (_luckyWheelHistoryRepositories.SaveChanges() > 0)
            {
                return Mapper.Map<LuckyWheelHistoryDto>(result);
            }

            return null;
        }

        public int DeleteLuckyHistory(int id)
        {
            _luckyWheelHistoryRepositories.Delete(a => a.Id == id);

            return _luckyWheelHistoryRepositories.SaveChanges();
        }

        public LuckyWheelItemDto RandomWheelItem(out int resultForWheel)
        {
            var wheelItems = _luckyWheelItemRepositories.Read(a => true).OrderBy(a => a.DisplayName);
            IList<ProportionValue<LuckyWheelItem>> listRandom = new List<ProportionValue<LuckyWheelItem>>();

            foreach (var item in wheelItems)
            {
                listRandom.Add(ProportionValue.Create((item.WinningPercent / 100) , item));
            }

            var result = listRandom.ChooseByRandom(out resultForWheel);

            return Mapper.Map<LuckyWheelItemDto>(result);
        }
    }
}
