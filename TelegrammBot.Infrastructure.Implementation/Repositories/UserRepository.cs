using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Entities;
using TelegrammBot.Infrastructure.Implementation.Repositories.Abstractions;
using TelegrammBot.Services.Abstractions.Interfaces;
using TelegrammBot.Services.Abstractions.Repositories.Interfaces;

namespace TelegrammBot.Infrastructure.Implementation.Repositories
{
    public class UserRepository(IApplicationContext context) : BaseRepository<User>(context), IUserRepository
    {
    }
}
