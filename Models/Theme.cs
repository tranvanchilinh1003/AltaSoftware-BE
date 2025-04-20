using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Theme
    {
        public Theme()
        {
            SystemSettings = new HashSet<SystemSetting>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<SystemSetting> SystemSettings { get; set; }
    }
}
