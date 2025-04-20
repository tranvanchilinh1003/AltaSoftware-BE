using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TopicRequest
    {
        [Required(ErrorMessage = "Tên chủ đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự")]
        public string Name { get; set; }

        public string? Description { get; set; }
        public DateTime? EndDate { get; set; }
        public string? File { get; set; }
        public bool Active { get; set; }
    }
}
