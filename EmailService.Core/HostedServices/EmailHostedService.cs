using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

using EmailService.Core.Common.Email.EmailProvider;
using EmailService.Core.Common.Email.Model;
using EmailService.Core.Interfaces;

using Microsoft.Extensions.Hosting;

namespace EmailService.Core.HostedServices
{
    public class EmailHostedService : IHostedService, IDisposable
    {
        private Task? _sendTask;
        private CancellationTokenSource? _cancellationToken;
        private readonly BufferBlock<EmailModel> _mailQueue;
        private readonly IEmailSender _mailSender;

        public EmailHostedService()
        {
            _mailSender = new MailJetProvider();
            _mailQueue = new BufferBlock<EmailModel>();
            _cancellationToken = new CancellationTokenSource();
        }
       
        //Method send emaail - wakeup BufferBlock
        public Task SendEmailAsync(EmailModel emailModel) => _mailQueue.SendAsync(emailModel);
        

        public void Dispose()
        {
            DestroyTask();
        }

        private void DestroyTask()
        {
            try
            {
                if(_cancellationToken != null)
                {
                    _cancellationToken.Cancel();
                    _cancellationToken = null;
                }
                Console.WriteLine("[EMAIL SERVICE] DESTROY SERVICE");
            }
            catch { }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[EMAIL SERVICE] START SERVICE");
            _sendTask = BackgroundSendEmailAsync(_cancellationToken!.Token);
            return Task.CompletedTask;
        }

        private async Task BackgroundSendEmailAsync(CancellationToken token)
        {
            //Listen when have singal from BufferBlock
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var email = await _mailQueue.ReceiveAsync();
                    await _mailSender.SendEmail(email);
                }
                catch(OperationCanceledException)
                {
                    break;
                }catch(Exception ex)
                {
                    Console.WriteLine($"[EMAIL SERVICE] Exception{ex.Message} ");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            DestroyTask();
            await Task.WhenAny(_sendTask!, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
