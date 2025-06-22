using Rtc.Domain.Entities;

namespace Rtc.Domain.Dtos
{
    public class CurrencyDto : BaseClass
    {
        public int Idx { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
  
    }
}
