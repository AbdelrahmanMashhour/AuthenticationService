using ReposatoryPatternWithUOW.Core.Models;
using ReposatoryPatternWithUOW.Core.OptionsPatternClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Interfaces
{
    public interface IToken
    {
        public static string GenerateToken(User? user = null, string? role = null, DateTime? expiresAt = null, TokenOptionsPattern? tokenOpts = null) { return string.Empty; }
    }
}
