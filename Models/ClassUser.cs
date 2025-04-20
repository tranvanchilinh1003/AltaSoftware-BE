namespace ISC_ELIB_SERVER.Models
{
    public class ClassUser
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? UserId { get; set; }

        public int? UserStatusId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual User? User { get; set; }
        public virtual UserStatus? UserStatus { get; set; }
    }
}
