using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Models
{
    public class EmailVerificationCode
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
        public virtual User User { get; set; }
    }
}
