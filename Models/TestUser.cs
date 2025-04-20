using ISC_ELIB_SERVER.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestUser
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int? UserId { get; set; }
        public StatusTestUserEnum Status { get; set; }

        public virtual Test? Test {  get; set; } 
        public virtual User? User { get; set; }
    }
}
