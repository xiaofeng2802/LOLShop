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
            AutomaticMigrationDataLossAllowed = true;
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


            //context.Set<Rank>().AddOrUpdate(new Rank[]
            //{
            //   new Rank()
            //   {
            //       RankName = "Chưa Có Rank",
            //       RankImage = "../Images/no-medal.png",
            //       CreatedDate = DateTime.Now,
            //        Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 30000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 1",
            //               Description = "For testing",
            //               RankId = 5
            //           }
            //       }

            //   },
            //   new Rank()
            //   {
            //       RankName = "Đồng",
            //       CreatedDate = DateTime.Now,
            //       RankImage = "../Images/coper-medal.jpg",
            //       Children = new List<Rank>() {
            //           new Rank()
            //           {
            //                    RankName = "Đồng 1",
            //                   CreatedDate = DateTime.Now,
            //                   RankImage = "../Images/coper-medal.jpg"
            //           },
            //            new Rank()
            //           {
            //                    RankName = "Đồng 2",
            //                   CreatedDate = DateTime.Now,
            //                   RankImage = "../Images/coper-medal.jpg"
            //           },
            //             new Rank()
            //           {
            //                    RankName = "Đồng 3",
            //                   CreatedDate = DateTime.Now,
            //                   RankImage = "../Images/coper-medal.jpg"
            //           }
            //       },
            //       Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 3000000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 2",
            //               Description = "For testing",
            //               RankId = 6
            //           }
            //       }
            //   },
            //   new Rank()
            //   {
            //       RankName = "Bạc",
            //       CreatedDate = DateTime.Now,
            //       RankImage = "../Images/silver-medal.png",
            //         Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {

            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 3",
            //               Description = "For testing",
            //               RankId = 7
            //           }
            //       }
            //   },
            //     new Rank()
            //   {
            //       RankName = "Vàng",
            //       CreatedDate = DateTime.Now,
            //       RankImage = "../Images/gold-medal.png",
            //         Accounts = new List<Account>()
            //       {
            //           new Account()
            //           {
            //               CreatedDate = DateTime.Now,
            //               Avatar = "../Images/yasou.jpg",
            //               Price = 300000,
            //               IsAvailable = true,
            //               IsHot = true,
            //               UserName = "ABC",
            //               Password = "123",
            //               Title = "So Hot 4",
            //               Description = "For testing",
            //               RankId = 7
            //           }
            //       }
            //   },
            //      new Rank()
            //   {
            //       RankName = "Bạch Kim",
            //       CreatedDate = DateTime.Now,
            //       RankImage = "../Images/platinum-medal.png"
            //   },
            //    new Rank()
            //   {
            //       RankName = "Kim Cương",
            //       CreatedDate = DateTime.Now,
            //       RankImage = "../Images/diamond-medal.png"
            //   },
            //    new Rank()
            //    {
            //        RankName = "Cao Thu",
            //        CreatedDate = DateTime.Now,
            //        RankImage = "../Images/gosu-medal.png"
            //    },
            //    new Rank()
            //    {
            //        RankName = "Thach Dau",
            //        CreatedDate = DateTime.Now,
            //        RankImage = "../Images/challenger-medal.png"
            //    }
            //});


        }
    }
}
