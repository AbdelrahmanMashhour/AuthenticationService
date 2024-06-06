using ReposatoryPatternWithUOW.Core.DTOs;
using ReposatoryPatternWithUOW.Core.ReturnedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Interfaces
{
    public interface IUserReposatory
    {
        public Task<bool> SignUpAsync(SignUpDto obj);
        public Task<LoginResult> LoginAsync(LoginDto obj);
        public Task<string> SendVerficationCode(string email, bool? IsForResetingPassword = false);

        public Task<bool> ValidateCode(string email, string code, bool isForResetPassword = false);
        public Task<bool> SignOut(string email, string token);
    }
}
