using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmailService.Core.Common.Email.Model;
using EmailService.Core.Interfaces;

using Mailjet.Client;

using Newtonsoft.Json.Linq;

namespace EmailService.Core.Common.Email.EmailProvider
{
    public class MailJetProvider : EmailSender.EmailSender,IEmailSender
    {
        protected override async Task Send(EmailModel email)
        {
            try
            {
                JArray jArray = new JArray();
                JArray attachments = new JArray();
                if (email.Attachment != null && email.Attachment.Count() > 0)
                {
                    email.Attachment.ToList().ForEach(attachment => attachments.Add(
                        new JObject
                        {
                            new JProperty("Content-Type",attachment.ContentType),
                            new JProperty("Filename",attachment.Name),
                            new JProperty("Content",Convert.ToBase64String(attachment.Data))
                        }));
                }
                jArray.Add(new JObject
                {
                    new JProperty("FromEmail","lojaesportivaunipampa@gmail.com"),
                    new JProperty("FromName","MK-SPORTS"),
                    new JProperty("Recipients",new JArray
                    {
                        new JObject {
                            new JProperty("Email",email.EmailAnddress),
                            new JProperty("Name",email.EmailAnddress),
                        }
                    }),
                    new JProperty("Subject",email.Subject),
                    new JProperty("Text-part",email.Body),
                    new JProperty("Html-part",email.Body), // use HTML format
                    new JProperty("Attachments",attachments)
                });
                var client = EmailSender.EmailSender.CreateMailJetClient();
                var request = new MailjetRequest
                {
                    Resource = Mailjet.Client.Resources.Send.Resource
                }
                .Property(Mailjet.Client.Resources.Send.Messages, jArray);
                var response = await client.PostAsync(request);
                Console.WriteLine($"Send Result {response.StatusCode} with message:{response.Content}");
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message.ToString());
                
            }
        }
    }
}
