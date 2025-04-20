using ISC_ELIB_SERVER.Enums;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestByStudentResponse
    {
        public StatusTestUserEnum Status { get; set; }
        public TestResponse? Test { get; set; }
        public UserResponse? User { get; set; }
    }
}
