using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Models
{
    public class User
    {

        public User(string userName, string password, string firstName, string lastName, string gendervalue, string adres, string zipCode, string dob, string phone, string email)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            GenderValue = gendervalue;
            Adres = adres;
            ZipCode = zipCode;
            DOB = dob;
            Phone = phone;
            Email = email;
        }


        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GenderValue { get; set; }

        public string Adres { get; set; }

        public string ZipCode { get; set; }

        public string DOB { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public static void WriteErrorLog(string strErrorText)
        {
            try
            {
                //DECLARE THE FILENAME FROM THE ERROR LOG
                string strFileName = "errorLog.txt";
                /*DECLARE THE FOLDER WHERE THE LOGFILE HAS TO BE STORED, IN THIS EXAMPLE WE CHOSE THE PATH OF THE CURRENT APPLICATION*/
                string strPath = AppDomain.CurrentDomain.BaseDirectory;
                //WRITE THE ERROR TEXT AND THE CURRENT DATE-TIME TO THE ERROR FILE
                System.IO.File.AppendAllText(strPath + "\\" + strFileName, strErrorText + " - " + DateTime.Now.ToString() + "\r\n");
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in WriteErrorLog: " + ex.Message);
            }
        }
    }
}
