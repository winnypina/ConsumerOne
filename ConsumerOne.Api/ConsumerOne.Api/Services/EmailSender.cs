using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ConsumerOne.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task Send(string toAddress,string subject, string message)
        {
            try
            {

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("contact@consumer.one"), Body = message, Subject = subject
                };
                mailMessage.To.Add(toAddress);
                

                using (var client = new SmtpClient("smtp.consumer.one"))
                {
                    client.UseDefaultCredentials = false;
                    client.Port = 3535;
                    client.Credentials = new NetworkCredential("contact@consumer.one", "Haverim3$");
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}