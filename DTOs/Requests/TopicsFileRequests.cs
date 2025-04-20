using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TopicsFileRequest
    {
        [Required(ErrorMessage = "TopicId không được để trống")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "FileUrl không được để trống")]
        public string FileUrl { get; set; }

        public string? FileName { get; set; }
        public bool Active { get; set; }
    }
}
