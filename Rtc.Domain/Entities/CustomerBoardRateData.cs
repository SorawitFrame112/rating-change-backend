using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public class CustomerBoardRateData
    {
        [Key]
        public int? idx { get; set; }
        public string CompanyName { get; set; }
        public int ExpectRate { get; set; }
        public int CurrentRate { get; set; }
      
    }
}
