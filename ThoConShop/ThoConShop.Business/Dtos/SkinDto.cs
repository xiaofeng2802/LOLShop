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

        public bool IsSpecial { get; set; }

        public string Avatar { get; set; }

        public int? GroupId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsOnFilter { get; set; }
    }
}
