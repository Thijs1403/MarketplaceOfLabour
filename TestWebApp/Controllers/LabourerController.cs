using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Models;
using TestWebApp.Pages;

namespace TestWebApp.Controllers
{
    public class LabourerController : Controller
    {
        public IActionResult Index()
        {
            //Return alle Labourers in een List
            MollShopContext context = HttpContext.RequestServices.GetService(typeof(TestWebApp.Models.MollShopContext)) as MollShopContext;
            return View(context.GetAllLabourers());
        }

        [HttpGet]
        public IActionResult Write()
        {
            //Get de View voor de pagina waarop Labourers kunnen worden geinsert in de database
            return View();

        }

        //losse parameters?
        [HttpPost]
        public IActionResult Write(int LabourerID, string FirstName, string LastName, int Gender, string Address, string ZipCode, string PhoneNumber, string Email )
        {
            //Insert een Labourer in de database
            Labourer lab = new Labourer(LabourerID, FirstName, LastName, Gender, Address, ZipCode, PhoneNumber, Email);
            MollShopContext context = HttpContext.RequestServices.GetService(typeof(TestWebApp.Models.MollShopContext)) as MollShopContext;
            context.WriteLabourer(lab);
            return RedirectToAction("Index", "Labourer");
        }
    }
}