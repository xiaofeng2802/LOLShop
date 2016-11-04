using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public class UserDto 
    {
        public string Id { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public byte[] Password { get; set; }

        public decimal? Balance { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
