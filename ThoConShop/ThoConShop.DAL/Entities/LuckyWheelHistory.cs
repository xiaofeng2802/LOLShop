using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DAL.Entities
{
    public class LuckyWheelHistory : BaseEntity<int>
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public string Result { get; set; }

    }
}
