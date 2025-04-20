using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TrainingProgram
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? MajorId { get; set; }
        public int? SchoolFacilitiesId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Degree { get; set; }
        public string? TrainingForm { get; set; }
        public bool? Active { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
