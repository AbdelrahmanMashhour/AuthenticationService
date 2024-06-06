using ReposatoryPatternWithUOW.Core.Interfaces;
using ReposatoryPatternWithUOW.Core.OptionsPatternClasses;
using ReposatoryPatternWithUOW.EF.Mapper;
using ReposatoryPatternWithUOW.EF.Reposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDbContext context;
        public IUserReposatory UserReposatory { get; }

        public UnitOfWork(AppDbContext context,Mapperly mapper,TokenOptionsPattern options,ISenderService senderService)
        {
            this.context = context;
            UserReposatory = new UserReposatory(context, mapper,options,senderService);
        }


        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
