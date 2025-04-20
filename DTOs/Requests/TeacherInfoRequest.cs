namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TeacherInfoRequest
    {
        public string? Cccd { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? IssuedPlace { get; set; }
        public bool? UnionMember { get; set; }
        public DateTime? UnionDate { get; set; }
        public string? UnionPlace { get; set; }
        public bool? PartyMember { get; set; }
        public DateTime? PartyDate { get; set; }
        public int? UserId { get; set; }
        public string? AddressFull { get; set; }
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int WardCode { get; set; }
        public bool Active { get; set; }
        public int SubjectId { get; set; } // Môn học
        public string? Position { get; set; } // Chức vụ
    }
}
