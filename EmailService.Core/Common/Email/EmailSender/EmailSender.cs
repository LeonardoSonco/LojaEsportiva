using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailService.Core.Common.Email.Model;
using EmailService.Core.Interfaces;

using Mailjet.Client;

namespace EmailService.Core.Common.Email.EmailSender
{
    public abstract class EmailSender : IEmailSender
    {

        public static MailjetClient CreateMailJetClient()
        {
            return new MailjetClient("ad0dee8a5ea3a1d00d3778d25ccad09b", "feb43967fe043d38cc642cfc50226efc");
        }

        protected abstract Task Send(EmailModel emailModel);

        public async Task SendEmail(EmailModel emailModel)
        {
            await Send(emailModel);
        }
        public  async Task SendEmail(string address, string subject, string body, List<EmailAttachment> emailAttachment = null)
        {
            await Send(new EmailModel
            {
                Attachment = emailAttachment!,
                Body = body,
                EmailAnddress = address,
                Subject = subject,
            });
        }

        Task IEmailSender.Send(EmailModel emailModel)
        {
            throw new NotImplementedException();
        }
    }
}
