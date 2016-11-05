using Microsoft.AspNet.Identity.EntityFramework;
using ThoConShop.DataSeedWork.Identity;
using ThoConShop.DAL.Contracts;

namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    public partial class ShopThoCon : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IShopConThoDbContext
    {
        public ShopThoCon(): base("ShopThoConDb")
        {

        }

        public ShopThoCon(string connectionString = "ShopThoConDb")
            : base(connectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ShopThoCon, ThoConShop.DAL.Migrations.Configuration>(connectionString));
        }

        public static ShopThoCon Create()
        {
            return new ShopThoCon();
        }

        public DbEntityEntry<TEntity> Entry<TKey, TEntity>(TEntity entity)
           where TKey : struct
           where TEntity : BaseEntity<TKey>
        {
            return this.Entry(entity);
        }

        public DbSet<TEntity> Set<TKey, TEntity>()
            where TKey : struct
            where TEntity : BaseEntity<TKey>
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Account>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Skins);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.UserTradingHistories)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rank>()
                .Property(e => e.RankImage)
                .IsUnicode(false);

            modelBuilder.Entity<Skin>()
                .HasMany(e => e.Accounts);

            modelBuilder.Entity<User>()
                .Property(e => e.Balance)
                .HasPrecision(19, 0);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserTradingHistories)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.ParValue)
                .HasPrecision(19, 0);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.PinNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.NumberOfPageGems)
                .WithRequired(a => a.Account)
                .HasForeignKey(a => a.AccountId);

            modelBuilder.Entity<UserTradingHistory>()
                .Ignore(a => a.Id);

            modelBuilder.Entity<Skin>()
                .HasMany(a => a.Children)
                .WithOptional(a => a.Parent)
                .HasForeignKey(a => a.GroupId);

            modelBuilder.Entity<Rank>()
              .HasMany(a => a.Children)
              .WithOptional(a => a.Parent)
              .HasForeignKey(a => a.GroupId);

            modelBuilder.Entity<Champion>()
                        .HasMany(a => a.Skins)
                        .WithOptional(a => a.Champion)
                        .HasForeignKey(a => a.BelongToChampion);

            modelBuilder.Entity<User>()
                .HasRequired(a => a.GeneralUser);

            modelBuilder.Entity<ApplicationUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<ApplicationUser>().HasKey(a => a.Id);




        }
    }
}
