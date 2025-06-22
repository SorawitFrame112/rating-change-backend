using Rtc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Dtos
{
    public class CurrencyDtos : BaseClass
    {
        public int Idx { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
  
    }
}
