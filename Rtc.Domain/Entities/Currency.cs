using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public class Currency:BaseClass
    {
        [Key]
        public int Idx { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
  
    }
}
