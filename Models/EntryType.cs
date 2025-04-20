using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class EntryType
    {
        public EntryType()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
