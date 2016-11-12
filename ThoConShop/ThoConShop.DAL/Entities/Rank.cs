namespace ThoConShop.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rank")]
    public partial class Rank : BaseEntity<int>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rank()
        {
            Accounts = new HashSet<Account>();
            Children = new HashSet<Rank>();
        }

        [StringLength(50)]
        public string RankName { get; set; }

        [StringLength(500)]
        public string RankImage { get; set; }

        public int? GroupId { get; set; }

        public virtual Rank Parent { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Rank> Children { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
