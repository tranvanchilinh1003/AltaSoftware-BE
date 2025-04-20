namespace ISC_ELIB_SERVER.DTOs.Requests
{
    namespace ISC_ELIB_SERVER.DTOs.Requests
    {
        public class TransferSchoolRequest
        {
            public int? StudentId { get; set; }

            public string? StudentCode { get; set; }
            public string? FullName { get; set; }

            public DateTime TransferSchoolDate { get; set; }
            public string? TransferToSchool { get; set; }
            public string? SchoolAddress { get; set; }
            public string? Reason { get; set; }
           
            public int? ProvinceCode { get; set; }
            public int? DistrictCode { get; set; }

            public string? AttachmentName { get; set; }
            public string? AttachmentPath { get; set; }
            public int? SemesterId { get; set; }  // Chuyển sang int (nếu BE yêu cầu)
            public int? UserId { get; set; }

        }
    }

}
