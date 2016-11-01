using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class SkinDto : BaseDto<int>
    {
        public string SkinName { get; set; }

        public bool? IsSpecial { get; set; }
    }
}
