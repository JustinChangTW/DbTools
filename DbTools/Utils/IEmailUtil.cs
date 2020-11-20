using System.Net.Mail;
using DbTools.Models;

namespace DbTools.Utils
{
    public interface IEmailUtil
    {
        void Config(SmtpModel smtpModel, MailMessage mailMessage);
        string SendEmaill();
    }
}