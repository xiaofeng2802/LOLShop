namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Skin")]
    public partial class Skin : BaseEntity<int>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Skin()
        {
            Accounts = new HashSet<Account>();
        }

        [StringLength(100)]
        public string SkinName { get; set; }

        public bool IsSpecial { get; set; } = false;

        public Skin Parent { get; set; }

        public string Avatar { get; set; }

        public bool IsDeleted { get; set; }

        public int? ChampionId { get; set; }

        public virtual Champion Champion { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
