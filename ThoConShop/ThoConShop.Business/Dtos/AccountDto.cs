using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class AccountDto : BaseDto<int>
    {

        public AccountDto()
        {
            Champions = new List<ChampionDto>();
            Skins = new List<SkinDto>();

        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public int RankId { get; set; }

        public string RankName { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsHot { get; set; }

        public string Avatar { get; set; }

        public int NumberOfPageGem { get; set; }
    
        public int NumberOfChamps { get; set; }

        public int NumberOfSkins { get; set; }

        public IList<SkinDto> Skins { get; set; }

        public IList<ChampionDto> Champions { get; set; }
    }
}
