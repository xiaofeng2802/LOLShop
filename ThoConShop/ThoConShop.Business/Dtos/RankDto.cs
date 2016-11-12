using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class RankDto : BaseDto<int>
    {
        public string RankName { get; set; }

        public string RankImage { get; set; }

        public int? GroupId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
