using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.DTOs
{
    public class SignUpDto
    {
        public string FirstName { get; set; }

        [Required]
        public string  LastName { get; set; }

        [RegularExpression(@"\w+@\w+\.\w+(\.\w+)*", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string confirmPassword { get; set; }

    }
}
