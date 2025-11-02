using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class EmailSenderService(IConfiguration configuration) : IEmailSenderService
{
    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");
        var fromAddress = smtpSettings["FromAddress"]!;
        var host = smtpSettings["Host"]!;
        var port = int.Parse(smtpSettings["Port"]!);
        var username = smtpSettings["Username"]!;
        var password = smtpSettings["Password"]!;

        var mailMessage = new MailMessage(from: fromAddress, to: to, subject: subject, body: body)
        {
            IsBodyHtml = true
        };

        using var smtpClient = new SmtpClient(host, port);
        smtpClient.Credentials = new NetworkCredential(username, password);
        smtpClient.EnableSsl = true;

        await smtpClient.SendMailAsync(mailMessage, ct);
    }
}