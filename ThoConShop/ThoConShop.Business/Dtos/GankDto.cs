using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class GankDto : BaseDto<int>
    {
        public string GankName { get; set; }

        public string GankImage { get; set; }
    }
}
