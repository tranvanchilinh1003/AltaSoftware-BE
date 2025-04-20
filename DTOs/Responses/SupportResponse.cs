using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SupportResponse
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public SupportTypeResponse? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public UserSuppostResponse User  { get; set; }
        public bool Active { get; set; }
    }

    public class UserSuppostResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string FullName { get; set; }
    }

    public class SupportTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
