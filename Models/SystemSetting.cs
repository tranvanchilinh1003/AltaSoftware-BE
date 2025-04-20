using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SystemSetting
    {
        public int Id { get; set; }
        public bool? Captcha { get; set; }
        public int? UserId { get; set; }
        public int? ThemeId { get; set; }
        public bool Active { get; set; }

        public virtual Theme? Theme { get; set; }
        public virtual User? User { get; set; }
    }
}
