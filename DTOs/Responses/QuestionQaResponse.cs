namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class QuestionQaResponse
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateAt { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserAvatar { get; set; }
    public int ViewCount { get; set; }
    public bool IsRead { get; set; }
    public bool HasAnswer { get; set; }
    public List<string>? ImageUrls { get; set; }
}

}
