namespace ISC_ELIB_SERVER.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpireDate { get; set; }

        public virtual User? User { get; set; }
    }
}
