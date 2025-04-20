namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TeacherFamilyResponse
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string GuardianName { get; set; } = string.Empty;
        public string GuardianPhone { get; set; } = string.Empty;
        public string GuardianAddressDetail { get; set; } = string.Empty;
        public string GuardianAddressFull { get; set; } = string.Empty;
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int WardCode { get; set; }

        public bool Active { get; set; }
    }
}
