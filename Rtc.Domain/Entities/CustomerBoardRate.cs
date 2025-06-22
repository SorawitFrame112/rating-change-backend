using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public class CustomerBoardRate:BaseClass
    {
        [Key]
        public int Idx { get; set; }
        public string FileName { get; set; }
        public int CustomerId { get; set; }
        public int CurrentcyId { get; set; }
      
    }
}
