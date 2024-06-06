using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.DTOs
{
    public class SendCodeDto
    {
        [RegularExpression(@"\w+@\w+\.\w+(\.\w+)*", ErrorMessage = "Invalid Email")]
        [StringLength(100)]

        public string Email { get; set; }
        public bool? Reset { get; set; }
    }
}
