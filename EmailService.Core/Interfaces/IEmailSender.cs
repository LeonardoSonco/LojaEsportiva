using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailService.Core.Common.Email.Model;

namespace EmailService.Core.Interfaces
{
    public interface IEmailSender
    {
        Task Send(EmailModel emailModel);

        //Email Sender Contract
        Task SendEmail(string address,string subject,string body, List<EmailAttachment>? emailAttachment = null);
        Task SendEmail(EmailModel emailModel);
    }
}
