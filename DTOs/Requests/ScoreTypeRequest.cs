
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ScoreTypeRequest
    {
        [Required(ErrorMessage = "Tên loại điểm không được trống!")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hệ số !")]
        [Range(1, 3, ErrorMessage = "Phạm vi hệ số từ 1 đến 3")]
        public int? Weight { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số cột điểm tối thiểu kì 1 không được âm!")]
        public int? QtyScoreSemester1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số cột điểm tối thiểu kì 2 không được âm!")]
        public int? QtyScoreSemester2 { get; set; }
    }
}

