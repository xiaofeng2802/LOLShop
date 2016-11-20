﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class LuckyWheelItemDto : BaseDto<int>
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }

    public class LuckyWheelHistoryDto : BaseDto<int>
    {
        public string UserName { get; set; }

        public string Result { get; set; }
    }
}