using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<ClassUser> ClassUsers { get; set; }
    }
}
