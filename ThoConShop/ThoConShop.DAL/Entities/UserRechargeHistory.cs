namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserRechargeHistory")]
    public partial class UserRechargeHistory : BaseEntity<int>
    {

        public int? UserId { get; set; }

        [Column(TypeName = "money")]
        public decimal ParValue { get; set; } = 0;

        [StringLength(50)]
        public string SerialNumber { get; set; }

        [StringLength(50)]
        public string PinNumber { get; set; }

        [StringLength(100)]
        public string Message { get; set; }

        public virtual User User { get; set; }
    }
}
