using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TestWebApp.Models
{
    public class Labourer
    {

        private MollShopContext context;
        private string labourerID;
        public Labourer() { }

        public Labourer(int labourerID, string firstName, string lastName, int gender, string address, string zipCode, string phoneNumber, string email)
        {
            LabourerID = labourerID;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Address = address;
            ZipCode = zipCode;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        [Required(ErrorMessage = "UserName is required")]
        public int LabourerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
