namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ClassUserResponse
    {
        public int Id { get; set; }
        public ClassRepspose? Class { get; set; }
        public UserRespose? User { get; set; }

        public UserStatusResponse? UserStatus { get; set; }
    }

    public class ClassRepspose
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
    }

    public class UserRespose
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
    }

}
