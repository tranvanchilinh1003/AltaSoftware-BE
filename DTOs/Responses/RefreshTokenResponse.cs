namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class RefreshTokenResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpireDate { get; set; }
    }
}
