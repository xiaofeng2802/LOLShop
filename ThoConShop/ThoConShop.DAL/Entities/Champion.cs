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
        [StringLength(50)]
        public string ChampionName { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
