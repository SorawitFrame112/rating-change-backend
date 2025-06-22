using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public  class BaseClass
    {
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdateBy { get; set; }
    }
}
