using BaseTest.Models.Entities;
using BaseTest.Models.Form;

namespace BaseTest.Businiss.MailService;

public interface IMailService
{
    void SendMail(MailForm mailForm);
    public bool ConfirmEmailByUrl(UserCard userCard,string token);
}