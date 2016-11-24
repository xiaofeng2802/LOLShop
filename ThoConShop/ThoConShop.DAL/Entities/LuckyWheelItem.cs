using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DAL.Entities
{
    public class LuckyWheelItem : BaseEntity<int>
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsUnlucky { get; set; }

        public float WinningPercent { get; set; }
    }
}
