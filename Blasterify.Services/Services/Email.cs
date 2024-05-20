using System.Net.Mail;
using System.Net;

namespace Blasterify.Services.Services
{
    public static class Email
    {
        public static void SentEmail(string toEmail, string displayName)
        {
            var fromAddress = new MailAddress("daoblur.business@gmail.com", "Blasterify.MVC");
            var toAddress = new MailAddress(toEmail, displayName);
            
            const string fromPassword = "tsckluvcqvnncwnl";
            const string subject = "Rent";
            const string body = "Dear Customer,\r\n\r\nWe hope you are well.\r\n\r\nWe noticed that you recently initiated a purchase process in our system, but were unable to complete it. We wanted to make sure you didn't have any problems and offer our help if you need it.\r\n\r\nWe understand that sometimes things don't go as we expect, but we are here to help you. If you have any questions or need assistance with your purchase, please do not hesitate to contact us. Our customer service team is always ready to help you.\r\n\r\nAdditionally, we would like to offer you a **10% discount** on your next purchase as a token of our appreciation for choosing us. Just use the discount code **\"REBUY10\"** at checkout.\r\n\r\nRemember, your satisfaction is our number one priority. We are committed to providing you with the best products and service possible.\r\n\r\nWe hope to see you soon in our store.\r\n\r\nBest regards from Blasterify.MVC.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}
