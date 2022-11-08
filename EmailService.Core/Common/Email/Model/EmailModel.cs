using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Core.Common.Email.Model
{
    public class EmailModel
    {
        public string EmailAnddress { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IEnumerable<EmailAttachment> Attachment { get; set; }
    }
    public class EmailAttachment
    {
        public string Name { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        public byte[] Data = new byte[0];
    }
}
