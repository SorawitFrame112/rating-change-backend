using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Idx { get; set; }
        public string RoleName { get; set; }

    }
}
