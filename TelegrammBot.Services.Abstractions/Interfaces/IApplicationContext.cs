using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegrammBot.Services.Abstractions.Interfaces
{
    /// <summary>
    /// EntityFramework context interface.
    /// </summary>
    public interface IApplicationContext
    {
        DbSet<T> Set<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
