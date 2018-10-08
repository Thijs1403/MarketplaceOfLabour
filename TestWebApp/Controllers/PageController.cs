using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Models;

namespace TestWebApp.Controllers
{
    public class PageController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string UserName, string Password, string FirstName, string LastName, string GenderValue, string Adres, string ZipCode, string DOB, string Phone, string Email)
        {
            User user = new User(UserName, Password, FirstName, LastName, GenderValue, Adres, ZipCode, DOB, Phone, Email);
            LoginModel loginMdl = new LoginModel(Email, Password);
            MollShopContext context = HttpContext.RequestServices.GetService(typeof(TestWebApp.Models.MollShopContext)) as MollShopContext;
            int userExists = context.CheckIfUserExists(loginMdl);
            if (userExists == 0)
            {
                context.RegisterNewUser(user);
                return View();

            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string Password, string EmailAddress)
        {
            LoginModel loginMdl = new LoginModel(EmailAddress, Password);
            MollShopContext context = HttpContext.RequestServices.GetService(typeof(TestWebApp.Models.MollShopContext)) as MollShopContext;
            int result = context.UserLogin(loginMdl);
            if (result == 1)
            {

                return RedirectToAction("LI_index", "Page");

            }

            else
            {
                return View();
            }
        }
        public IActionResult LI_index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}