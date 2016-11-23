using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{

    public class UserTopCharging
    {
        public string UserName { get; set; }

        public float SumOfMonth { get; set; }
    }
    public class UserDto : BaseDto<int>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public float Balance { get; set; } = 0;

        public bool IsActive { get; set; }

        public string GeneralUserId { get; set; }

        public string NameDisplay { get; set; }
    }
}
