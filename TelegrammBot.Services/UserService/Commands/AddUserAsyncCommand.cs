using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.EntitiesDto;

namespace TelegrammBot.Services.UserService.Commands
{
    public sealed record AddUserAsyncCommand(UserDto User) : IRequest<UserDto>;
}
