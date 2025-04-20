using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class StudentScoreRequest
    {
        [Required(ErrorMessage = "Điểm số không được trống!")]
        [Range(0, 10, ErrorMessage = "Điểm số phải trong khoảng từ 0 đến 10")]
        public double? Score { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại điểm!")]
        public int ScoreTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học!")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học sinh!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học kỳ!")]
        public int SemesterId { get; set; }
    }
}
