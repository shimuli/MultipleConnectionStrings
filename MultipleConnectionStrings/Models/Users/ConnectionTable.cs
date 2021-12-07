using System;
using System.Collections.Generic;

#nullable disable

namespace MultipleConnectionStrings.Models.Users
{
    public partial class ConnectionTable
    {
        public ConnectionTable()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string ConnectionName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
