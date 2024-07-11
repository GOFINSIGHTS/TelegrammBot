using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Abstractions;

namespace TelegrammBot.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required long ChatId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Message { get; set; }

        [Required]
        public required DateTime Date { get; set; }
    }
}
