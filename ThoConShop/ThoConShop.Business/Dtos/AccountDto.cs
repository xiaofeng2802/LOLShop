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

        public string AccountName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int? GankId { get; set; }

        public string Description { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsHot { get; set; }

        public string Avatar { get; set; }

        public byte? NumberOfPageGem { get; set; }

        public IList<SkinDto> Skins { get; set; }

        public IList<ChampionDto> Champions { get; set; }
    }
}
