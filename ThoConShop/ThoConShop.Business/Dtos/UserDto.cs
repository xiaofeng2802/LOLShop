﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class UserDto : BaseDto<int>
    {

        public decimal Balance { get; set; } = 0;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string GeneralUserId { get; set; }

        public string NameDisplay { get; set; }
    }
}