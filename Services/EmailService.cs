using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["Smtp:Server"];
        _smtpPort = int.Parse(configuration["Smtp:Port"]);
        _smtpUser = configuration["Smtp:User"];
        _smtpPass = configuration["Smtp:Pass"];
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ISC LMS", _smtpUser));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            string emailType = subject.Contains("OTP") ? "OTP" : "temporaryPassword";
            string emailBodyContent = body; 
            string validityMessage = string.Empty;

            if (emailType == "temporaryPassword")
            {
                validityMessage = "Mật khẩu tạm thời chỉ tồn tại được <strong>30 phút</strong>.";
            }
            else if (emailType == "OTP")
            {
                validityMessage = "OTP chỉ tồn tại được <strong>3 phút</strong>.";
            }

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "EmailTemplate.html");
            var template = await File.ReadAllTextAsync(templatePath);

            var htmlBody = template
                .Replace("{{body}}", emailBodyContent)
                .Replace("{{validityMessage}}", validityMessage)
                .Replace("{{subject}}", subject); 

            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}
