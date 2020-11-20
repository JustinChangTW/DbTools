using DbTools.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace DbTools.Utils
{
    public class EmailUtil : IEmailUtil
    {
        SmtpModel smtpModel;
        MailMessage mailMessage;
        private ILogger<EmailUtil> logger;
        public EmailUtil(ILogger<EmailUtil> logger) { this.logger = logger; }

        public void Config(SmtpModel smtpModel,MailMessage mailMessage)
        {
            this.smtpModel = smtpModel;
            this.mailMessage = mailMessage;
        }

        //EMAIL
        public string SendEmaill()
        {
            try
            {
                using (var client = new SmtpClient(smtpModel.SmtpServer, smtpModel.SmtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpModel.SmtpUser,smtpModel.SmtpPassword);
                    client.Send(mailMessage);
                    logger.LogInformation($"郵件寄送成功!!{mailMessage}");
                }
            }
            catch
            {
                logger.LogInformation("郵件寄送失敗!!");
                return "郵件寄送失敗，請到系統內去觀看!!";
            }
            return "郵件寄送成功";
        }
    }
}
