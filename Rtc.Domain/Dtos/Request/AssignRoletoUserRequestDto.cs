using Rtc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Domain.Dtos
{
    public class AssignRoletoUserRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

    }
}
