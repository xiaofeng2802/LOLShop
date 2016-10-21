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
        }

        [StringLength(50)]
        public string UserName { get; set; }

        public byte[] Password { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRechargeHistory> UserRechargeHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTradingHistory> UserTradingHistories { get; set; }
    }
}
