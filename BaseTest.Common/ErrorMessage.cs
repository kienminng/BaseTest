using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Common
{
    public class ErrorMessage
    {
        public class ProductErrorMessage
        {
            public const string ProductNameWasExisted = "product name was existed please try again with another name";
            public const string PriceError = "Product price must be greater than 0";
            public const string Discount = "The discount percentage must be greater than or equal to 0";
            public const string ProductNotExisted = "Product was't existed";
        }
        
        public class UserErrorMessage
        {
            public const string UserNotFound = " Your account user not found";
            public const string UserWasBand = "Your Account user was band , please contact with us url={?} ";
            public const string WrongPassword = "Your password is not correct";
            public const string EmailIsExisted = "Your email was exist please try with another ";
        }
    }
}
