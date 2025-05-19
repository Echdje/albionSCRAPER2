using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
using albionSCRAPERV2.Models;

namespace albionSCRAPERV2.Services;
public class MailNotificationService
{
    private readonly EmailConfig _emailConfig;

    public MailNotificationService()
    {

        _emailConfig = LoadEmailConfig();
    }

    private EmailConfig LoadEmailConfig()
    {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "albionSCRAPERV2.Data.mailconfig.json";

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException("Brak pliku konfiguracyjnego dla emaila. Sprawdz Data/");
            }

            using var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            Console.WriteLine("mailconfig poprawnie zaladowany");
            EmailConfig? config = JsonSerializer.Deserialize<EmailConfig>(json);

            return config ?? throw new InvalidOperationException("nie udalo sie zdeserializowac");

    }

    public async Task SendNotificationAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage()
        {
            From = new MailAddress(_emailConfig.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
        {
            Port = _emailConfig.Port,
            Credentials = new NetworkCredential(_emailConfig.SenderEmail, _emailConfig.SenderPassword),
            EnableSsl = _emailConfig.EnableSsl
        };
        
        await client.SendMailAsync(message);
    }

}