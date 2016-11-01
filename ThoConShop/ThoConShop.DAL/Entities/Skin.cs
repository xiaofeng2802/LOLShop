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
            Children = new HashSet<Skin>();
        }

        [StringLength(100)]
        public string SkinName { get; set; }

        public bool? IsSpecial { get; set; }

        public int? GroupId { get; set; }

        public Skin Group { get; set; }

        public ICollection<Skin> Children { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
