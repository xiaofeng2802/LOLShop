﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.DataSeedWork.Extensions
{
    public static class DoubleExtension
    {
        public static string VietNameseMoneyFormat(this float value)
        {
            if (value <= 0)
            {
                return "0";
            }
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"

            return value.ToString("#,###",cul.NumberFormat);
        }
    }
}
