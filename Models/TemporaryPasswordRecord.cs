namespace ISC_ELIB_SERVER.Models
{
    public class TemporaryPasswordRecord
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
