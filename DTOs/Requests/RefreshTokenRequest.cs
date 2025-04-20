namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public int Id {  get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpireDate { get; set; }
    }
}
