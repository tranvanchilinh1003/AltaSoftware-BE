namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SystemSettingResponse
    {
        public bool? Captcha { get; set; }
        public int? UserId { get; set; }
        public int? ThemeId { get; set; }
        public bool Active { get; set; }
    }
}