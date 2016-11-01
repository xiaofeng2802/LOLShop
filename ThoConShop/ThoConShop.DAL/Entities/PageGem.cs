using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DAL.Entities
{
    public class PageGem : BaseEntity<int>
    {
        public string ImageUrl { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
