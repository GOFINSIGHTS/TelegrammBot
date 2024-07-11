using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Entities;
using TelegrammBot.Services.Abstractions.Interfaces;

namespace TelegrammBot.Infrastructure.PostgreSql.Persistance
{
    public sealed class TelegrammContext(DbContextOptions<TelegrammContext> options) : DbContext(options), IApplicationContext
    {
        public DbSet<User> Users { get; set; }
    }
}
