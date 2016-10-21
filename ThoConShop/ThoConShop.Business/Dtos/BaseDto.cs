using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoConShop.Business.Dtos
{
    public abstract class BaseDto<TKey> where TKey : struct 
    {
        public TKey Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
