using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Abstractions;

namespace TelegrammBot.Domain.EntitiesDto
{
    public class UserDto : BaseEntityDto
    {        
        public required long ChatId { get; set; }
        
        public required string Message { get; set; }
        
        public required DateTime Date { get; set; }
    }
}
