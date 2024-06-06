using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Interfaces
{
    public interface ISenderService
    {
        public Task SendEmailAsync(string emailTo, string subject, string body);

    }
}
