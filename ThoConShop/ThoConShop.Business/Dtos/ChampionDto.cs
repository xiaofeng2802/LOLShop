using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class ChampionDto : BaseDto<int>
    {
        public string ChampionName { get; set; }

        public string Avatar { get; set; }

        public bool IsDeleted { get; set; }
    }
}
