using EmployeeManager.Domain.Interfaces;

namespace EmployeeManager.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            // Mock envia e-mail para logs ou console (substitua por SendGrid, SMTP, etc.)
            Console.WriteLine($"📨 E-mail enviado para {to}");
            Console.WriteLine($"Assunto: {subject}");
            Console.WriteLine($"Corpo: {body}");

            return Task.CompletedTask;
        }
    }
}
