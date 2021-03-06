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
            NumberOfPageGems = new HashSet<PageGem>();
        }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public float Price { get; set; }

        public int RankId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsHot { get; set; } = false;

        public string Avatar { get; set; }

        public int EventPrice { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skin> Skins { get; set; }

        public virtual Rank Rank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTradingHistory> UserTradingHistories { get; set; }

        public virtual ICollection<Champion> Champions { get; set; }

        public virtual ICollection<PageGem> NumberOfPageGems { get; set; }
    }
}
