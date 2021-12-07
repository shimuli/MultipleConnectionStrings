using System;
using System.Collections.Generic;

#nullable disable

namespace MultipleConnectionStrings.Models.Users
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ConnectionId { get; set; }
        public DateTime? DatePosted { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ConnectionTable Connection { get; set; }
    }
}
