using ThoConShop.DAL.Contracts;

namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Infrastructure;

    public partial class ShopThoCon : DbContext, IShopConThoDbContext
    {
        public ShopThoCon(string connectionString)
            : base(connectionString)
        {
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

            modelBuilder.Entity<Gank>()
                .Property(e => e.GankImage)
                .IsUnicode(false);

            modelBuilder.Entity<Skin>()
                .HasMany(e => e.Accounts);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Balance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserTradingHistories)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.ParValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserRechargeHistory>()
                .Property(e => e.PinNumber)
                .IsUnicode(false);
        }
    }
}
