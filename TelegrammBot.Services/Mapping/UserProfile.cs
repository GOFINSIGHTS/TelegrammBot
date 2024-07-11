using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Entities;
using TelegrammBot.Domain.EntitiesDto;

namespace TelegrammBot.Services.Mapping
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
            CreateMap<User, UserDto>();
        }
    }
}
