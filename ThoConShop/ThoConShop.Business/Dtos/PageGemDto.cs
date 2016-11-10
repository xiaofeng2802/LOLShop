using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ThoConShop.Business.Dtos
{
    public class PageGemDto : BaseDto<int>
    {
        public string ImageUrl { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public int AccountId { get; set; }
    }
}
