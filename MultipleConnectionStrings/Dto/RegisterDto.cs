using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleConnectionStrings.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ConnectionId { get; set; }
        //public DateTime? DatePosted { get; set; }
        //public DateTime? DateUpdated { get; set; }
    }
}
