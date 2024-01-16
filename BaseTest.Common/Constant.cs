using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Common
{
    public class Constant
    {
        public const string Bearer = "Bearer ";
        public const string DefaultRouter = "api/[Controller]/[action]";
        public const string RegisterConfirmUrl = "https://localhost:7174/User/api/";
        public static class AppSettingKeys
        {
            public const string DEFAULT_CONNECTION = "DefaultConnection";
            public const string DEFAULT_CONTROLLER_ROUTE = "api/[controller]/[action]";
            public const string SECRET_KEY = "AuthSecret";
            public const string Issuer = "";
            public const string ConfigDefault = "AppConfig";
            public const string Time = "TokenExpireTimeMinites";
        }
        
        
        public static class EmailInformation
        {
            public const string Default = "EmailInformation";
            public const string Form = "Form";
            public const string SmtpServer = "SmtpServer";
            public const string Port = "Port";
            public const string Username = "Username";
            public const string Password = "Password";
        }
        
        public static class User
        {

        }

        public static class Product
        {

        }

        public static class Payment
        {

        }

        public static class  Order
        {
            public const string ActualPriceErrorMessage = "Actual price must be larger than 0";
            public const string OriginPriceErrorMessage = "Origin price must be larger than 0";
        }
        
        public static class  Roles
        {
            public const string ClaimTypeId = "Id";
            public const string RoleAdmin = "Admin";
            public const string RoleUser = "User";
        }
        
        public static class Html
        {
            public const string Button =
                "<button style=\"background-color: #008CBA; border-radius: 20px; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer; \" type=\"submit\">Confirm</button>";
            
        }
        
        public static class Url
        {
            public const string ConfirmMail = "https://localhost:7174/api/User/ActiveUser";
            public const string RestPassword = "https://localhost:7174/api/User/ResetPassword";
        }


    }
}
