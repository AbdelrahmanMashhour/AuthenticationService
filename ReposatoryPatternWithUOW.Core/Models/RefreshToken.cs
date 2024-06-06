using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Models
{
    public class RefreshToken
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiresAt;
        public bool IsActive => !Revoked || !IsExpired;
        public virtual User User { get; set; }
    }
}
