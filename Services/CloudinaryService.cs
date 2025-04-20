using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DotNetEnv;


public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        Env.Load();
        _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
    }

    public async Task<string> UploadBase64Async(string base64, string fileName = null)
    {
        if (base64.Contains(','))
            base64 = base64.Substring(base64.IndexOf(',') + 1);

        var bytes = Convert.FromBase64String(base64);
        using var stream = new MemoryStream(bytes);

        var typeFile = GetTypeFile(base64);
        var timeStr = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var finalName = fileName ?? $"upload_{timeStr}{typeFile}";
        var isImage = typeFile is ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp";

        if (isImage)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(finalName, stream)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
        else
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(finalName, stream)
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl.ToString();
        }
    }


    private string GetTypeFile(string base64)
    {
        if (base64.StartsWith("/9j/")) return ".jpg";
        if (base64.StartsWith("iVBOR")) return ".png";
        if (base64.StartsWith("R0lGOD")) return ".gif";
        if (base64.StartsWith("UEsDB")) return ".docx";
        if (base64.StartsWith("JVBER")) return ".pdf";
        return ".bin";
    }

}
