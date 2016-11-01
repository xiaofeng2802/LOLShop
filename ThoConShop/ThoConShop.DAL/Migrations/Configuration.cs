using System.Collections.Generic;
using ThoConShop.DAL.Entities;

namespace ThoConShop.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ThoConShop.DAL.Entities.ShopThoCon>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ThoConShop.DAL.Entities.ShopThoCon context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.Set<Skin>().AddOrUpdate(new Skin[]
            //{
            //    new Skin()
            //    {
            //        SkinName = "Skin tối thượng (799)",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true,
            //        Children = new List<Skin>()
            //        {
            //            new Skin()
            //            {
            //                 SkinName = "Skin Ezreal Toi Thuong",
            //                 CreatedDate = DateTime.Now,
            //                 IsSpecial = true,
            //                 GroupId = 1
            //            }
            //        }
            //    },
            //     new Skin()
            //    {
            //        SkinName = "Skin Hextech",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    },
            //      new Skin()
            //    {
            //        SkinName = "Skin Quán Quân",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    },
            //       new Skin()
            //    {
            //        SkinName = "Skin Vinh Quang",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    },
            //        new Skin()
            //    {
            //        SkinName = "Skin Halloween",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    },
            //         new Skin()
            //    {
            //        SkinName = "Skin Thách Đấu",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    },
            //     new Skin()
            //    {
            //        SkinName = "Skin tối thượng (799)",
            //        CreatedDate = DateTime.Now,
            //        IsSpecial = true
            //    }
            //});


            //context.Set<Gank>().AddOrUpdate(new Gank[]
            //{
            //   new Gank()
            //   {
            //       GankName = "Chưa Có Gank",
            //       GankImage = "../Images/no-medal.png",
            //       CreatedDate = DateTime.Now,
            //        Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               AccountName = "Thuan",
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 1",
            //               Description = "For testing",
            //               GankId = 1
            //           }
            //       }

            //   },
            //   new Gank()
            //   {
            //       GankName = "Đồng",
            //       CreatedDate = DateTime.Now,
            //       GankImage = "../Images/coper-medal.jpg",
            //       Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               AccountName = "Thuan",
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 2",
            //               Description = "For testing",
            //               GankId = 2
            //           }
            //       }
            //   },
            //   new Gank()
            //   {
            //       GankName = "Bạc",
            //       CreatedDate = DateTime.Now,
            //       GankImage = "../Images/silver-medal.png",
            //         Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               AccountName = "Thuan",
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 3",
            //               Description = "For testing",
            //               GankId = 3
            //           }
            //       }
            //   },
            //     new Gank()
            //   {
            //       GankName = "Vàng",
            //       CreatedDate = DateTime.Now,
            //       GankImage = "../Images/gold-medal.png",
            //         Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               AccountName = "Thuan",
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 4",
            //               Description = "For testing",
            //               GankId = 4
            //           }
            //       }
            //   },
            //      new Gank()
            //   {
            //       GankName = "Bạch Kim",
            //       CreatedDate = DateTime.Now,
            //       GankImage = "../Images/platinum-medal.png"
            //   },
            //    new Gank()
            //   {
            //       GankName = "Kim Cương",
            //       CreatedDate = DateTime.Now,
            //       GankImage = "../Images/silver-medal.png"
            //   },
            //    new Gank()
            //    {
            //        GankName = "Cao Thu",
            //        CreatedDate = DateTime.Now,
            //        GankImage = "../Images/silver-medal.png"
            //    },
            //    new Gank()
            //    {
            //        GankName = "Thach Dau",
            //        CreatedDate = DateTime.Now,
            //        GankImage = "../Images/silver-medal.png"
            //    }
            //});


        }
    }
}
