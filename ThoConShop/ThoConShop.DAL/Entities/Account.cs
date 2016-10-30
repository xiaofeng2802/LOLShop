namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account : BaseEntity<int>
    {
        public Account()
        {
            Skins = new HashSet<Skin>();
            UserTradingHistories = new HashSet<UserTradingHistory>();
            Champions = new HashSet<Champion>();
        }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? GankId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsHot { get; set; }


        [StringLength(100)]
        public string Avatar { get; set; }

        public byte? NumberOfPageGem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skin> Skins { get; set; }

        public virtual Gank Gank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTradingHistory> UserTradingHistories { get; set; }

        public virtual ICollection<Champion> Champions { get; set; }
    }
}
