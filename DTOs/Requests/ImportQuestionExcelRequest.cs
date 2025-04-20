using System.ComponentModel.DataAnnotations;

public class ImportQuestionExcelRequest
{
    [Required]
    public IFormFile File { get; set; }

    [Required]
    public int TestId { get; set; }

    [Required]
    public string QuestionType { get; set; } // "TracNghiem" hoặc "TuLuan"
}
