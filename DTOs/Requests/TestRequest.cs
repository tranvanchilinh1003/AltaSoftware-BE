using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestRequest
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} không được để trống.")]

        [Range(0, 1, ErrorMessage = "{0} phải truyền từ 0 đến 1.")]
        public int? Type { get; set; }

        //[Range(1, 360, ErrorMessage = "Thời gian phải trong khoảng 1 đến 360 phút.")]
        public int? DurationTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(0,4,ErrorMessage = "{0} phải truyền từ 0 đến 4.")]
        public int? Classify { get; set; }
        public DateTime? StartTime { get; set; }
        [EndTimeAfferStartTimeAtribute("StartTime")]
        public DateTime? EndTime { get; set; }

        public string? File { get; set; }
        public string? Description { get; set; }

        //[Required(ErrorMessage = "Phải có ít nhất một lớp tham gia.")]
        public string? ClassIds { get; set; }

        public bool? FileSubmit { get; set; }

        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public long GradeLevelsId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public long SubjectId { get; set; }
        [Required(ErrorMessage = "{0} không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int? TeacherId { get; set; }
    }

    public class EndTimeAfferStartTimeAtribute : ValidationAttribute
    {
        private readonly string _startTimePropertyName;

        public EndTimeAfferStartTimeAtribute(string startTimePropertyName)
        {
            _startTimePropertyName = startTimePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endTime = value as DateTime?;
            var startTimeProperty = validationContext.ObjectType.GetProperty(_startTimePropertyName);
            if (startTimeProperty == null) 
            {
                return new ValidationResult($"Unknown property: {_startTimePropertyName}");
            }
            var startTime = startTimeProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (!startTime.HasValue || !endTime.HasValue) 
            {
                return ValidationResult.Success;
            }
            if( endTime > startTime)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("EndTime phải lớn hơn StartTime");
        }
    }
}
