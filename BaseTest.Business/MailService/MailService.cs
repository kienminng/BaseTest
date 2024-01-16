using BaseTest.Common;
using BaseTest.Models.Entities;
using BaseTest.Models.Form;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace BaseTest.Businiss.MailService;

public class MailService : IMailService
{
    private readonly MailInformation _emailConfig;

    public MailService(MailInformation emailConfig)
    {
        _emailConfig = emailConfig;
    }
    
    
    public void SendMail(MailForm mailForm)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("", _emailConfig.From));
        message.To.Add(new MailboxAddress("",mailForm.To ));
        message.Subject = mailForm.Subject;
        message.Body = new TextPart(TextFormat.Html) { Text = mailForm.Body };
        
        Connect(message);
    }
    
    public bool ConfirmEmailByUrl(UserCard user,string token)
    {
        
        var Url = $"{Constant.RegisterConfirmUrl}?Token={token}";
        var message = new MimeMessage();
        var tokenWithBearer = Constant.Bearer + token;

        message.From.Add(new MailboxAddress("", _emailConfig.From));
        message.To.Add(new MailboxAddress(user.Email, user.Email));
        message.Subject = "Welcome to Poly Food";
        message.Body = new TextPart(TextFormat.Html)
        {
            Text = "<h1> Welcome " + user.Username + " to join with us </h1> " +
                   "<h2> Please click the button to start</h2>" +
                   $"<a href=\"{Url}\">{Constant.Html.Button}</a>" +
                   "<h3> This is confidential information. </h3>" +
                   "<h3> Do not share it with anyone </h3>"
        };
        
        return Connect(message);
    }

    private bool Connect(MimeMessage message)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port);
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Send(message);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                client.Disconnect(true);
            }
        }
    }
}