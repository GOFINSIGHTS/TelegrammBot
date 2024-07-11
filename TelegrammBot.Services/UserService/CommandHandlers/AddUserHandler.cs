using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Entities;
using TelegrammBot.Domain.EntitiesDto;
using TelegrammBot.Services.Abstractions.Repositories.Interfaces;
using TelegrammBot.Services.UserService.Commands;

namespace TelegrammBot.Services.UserService.CommandHandlers
{
    public sealed class AddUserHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<AddUserAsyncCommand, UserDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDto> Handle(AddUserAsyncCommand request, CancellationToken cancellationToken)
        {            
            request.User.Date = DateTime.UtcNow;

            var message = request.User.Message;
            var range = message.Length > 254 ? 254 : message.Length;
            request.User.Message = message[..range];

            _userRepository.Add(_mapper.Map<User>(request.User));

            await _userRepository.SaveChangesAsync(cancellationToken);

            return request.User;
        }
    }
}
