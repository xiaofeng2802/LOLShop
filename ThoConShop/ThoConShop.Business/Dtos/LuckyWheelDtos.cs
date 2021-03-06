﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ThoConShop.Business.Dtos
{
    public class LatestUsingWheel
    {
        public string NameDisplay { get; set; }

        public string Result { get; set; }
    }

    public class LuckyWheelItemDto : BaseDto<int>
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsUnlucky { get; set; }

        public HttpPostedFileBase FileImage { get; set; }

        public float WinningPercent { get; set; }
    }

    public class LuckyWheelHistoryDto : BaseDto<int>
    {
        public int UserId { get; set; }
        public string NameDisplay { get; set; }

        public string Result { get; set; }
    }
}
