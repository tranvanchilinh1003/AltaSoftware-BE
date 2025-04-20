namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class UpdatePasswordByEmailRequest
    {
        public string Email { get; set; }
        public string TemporaryPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
