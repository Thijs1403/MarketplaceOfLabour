using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Models
{
    public class LoginModel
    {
        public LoginModel()
        {

        }
        public LoginModel(string emailaddress, string password)
        {
            EmailAddress = emailaddress;
            Password = password;
        }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
