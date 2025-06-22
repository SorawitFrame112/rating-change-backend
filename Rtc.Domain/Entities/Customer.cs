using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public  class Customer: BaseClass
    {
        [Key]
        public int Idx { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string TaxId { get; set; }
    }
}
