using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserReposatory UserReposatory { get; }
        public Task<int> SaveChangesAsync();
    }
}
