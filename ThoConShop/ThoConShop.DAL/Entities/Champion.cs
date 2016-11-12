using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DAL.Entities
{
    public class Champion : BaseEntity<int> 
    {

        public Champion()
        {
            Skins = new HashSet<Skin>();
            Accounts = new HashSet<Account>();
        }

        [StringLength(50)]
        public string ChampionName { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<Skin> Skins { get; set; }
    }
}
