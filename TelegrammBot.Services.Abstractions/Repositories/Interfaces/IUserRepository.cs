using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Entities;
using TelegrammBot.Services.Abstractions.Repositories.Interfaces.Abstractions;

namespace TelegrammBot.Services.Abstractions.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
