using ReposatoryPatternWithUOW.Core.DTOs;
using ReposatoryPatternWithUOW.Core.Models;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.EF.Mapper
{
    //[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
    [Mapper]
    public partial class Mapperly
    {
        public partial User MapToUser(SignUpDto signUpDto);
    }
}
