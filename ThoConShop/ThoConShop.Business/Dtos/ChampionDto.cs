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

    public class ChampUploadDto
    {
        public string id { get; set; }

        public int key { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public ImageUpoloadDto image { get; set; }

        public IList<SkinDetailUploadDto> skins { get; set; }
    }

    public class ImageUpoloadDto
    {
        public string full { get; set; }

        public string sprite { get; set; }
    }
}
