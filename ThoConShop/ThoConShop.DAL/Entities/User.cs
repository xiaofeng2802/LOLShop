using ThoConShop.DataSeedWork.Identity;

namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User : BaseEntity<int>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            UserRechargeHistories = new HashSet<UserRechargeHistory>();
            UserTradingHistories = new HashSet<UserTradingHistory>();
            LuckyWheelHistories = new HashSet<LuckyWheelHistory>();
        }

        public float Balance { get; set; } = 0;

        public int Point { get; set; }

        public string NameDisplay { get; set; }

        public bool IsActive { get; set; } = true;

        public string GeneralUserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRechargeHistory> UserRechargeHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTradingHistory> UserTradingHistories { get; set; }

        public virtual ApplicationUser GeneralUser { get; set; }

        public virtual ICollection<LuckyWheelHistory> LuckyWheelHistories { get; set; }
    }
}
