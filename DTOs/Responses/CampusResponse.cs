using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class CampusResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        // public int UserId { get; set; }
        public UserCampusResponse? User { get; set; }
    }

    public class UserCampusResponse
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

}
