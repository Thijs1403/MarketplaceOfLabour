using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TestWebApp.Models
{
    public class MollShopContext
    {
        //Errorlogging
        public static void WriteErrorLog(string strErrorText)
        {
            try
            {
                string strFileName = "errorLog.txt";
                string strPath = "C:\\Users\\Gebruiker\\Downloads\\MOLL0.0.8_-_Register_werkend (1)\\MOLL0.0.8 - Register werkend\\TestWebApp";

                System.IO.File.AppendAllText(strPath + "\\" + strFileName, strErrorText);
            }
            catch (Exception)
            {
                //WriteErrorLog("Error: " + ex.Message);
            }
        }
        //Encryptor
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //Decryptor
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public string ConnectionString { get; set; }

        public MollShopContext(string connectionString)
        {
            this.ConnectionString = "server=84.246.4.143;port=9139;Database=koster3mollshop;Uid=Koster3molladmin;Pwd=pi9!rtd@;SslMode=none;";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Labourer> GetAllLabourers()
        {
            List<Labourer> list = new List<Labourer>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbl_labourerdata", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Labourer()
                        {
                            LabourerID = Convert.ToInt32(reader["fld_LabourerId"]),
                            FirstName = reader["fld_FirstName"].ToString(),
                            LastName = reader["fld_LastName"].ToString(),
                            Gender = Convert.ToInt32(reader["fld_Gender"]),
                            Address = reader["fld_Address"].ToString(),
                            ZipCode = reader["fld_ZipCode"].ToString(),
                            PhoneNumber = reader["fld_PhoneNumber"].ToString(),
                            Email = reader["fld_email"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public void WriteLabourer(Labourer lab)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string commandString = "INSERT INTO tbl_labourerdata (fld_FirstName,fld_LastName,fld_Gender,fld_Address,fld_ZipCode,fld_PhoneNumber,fld_Email)"
                    + "VALUES(@firstName,@lastName,@gender,@address,@zipCode,@phoneNumber,@email)";
                MySqlCommand cmd = new MySqlCommand(commandString, conn);

                cmd.Parameters.AddWithValue("@firstName", lab.FirstName);
                cmd.Parameters.AddWithValue("@lastName", lab.LastName);
                cmd.Parameters.AddWithValue("@gender", lab.Gender);
                cmd.Parameters.AddWithValue("@address", lab.Address);
                cmd.Parameters.AddWithValue("@zipCode", lab.ZipCode);
                cmd.Parameters.AddWithValue("@phoneNumber", lab.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", lab.Email);

                cmd.ExecuteNonQuery();
            }
        }

        public void RegisterNewUser(User user)
        {

            try {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string commandString = "INSERT INTO tbl_userdata (fld_UserName,fld_Password,fld_FirstName,fld_LastName,fld_Gender,fld_Address,fld_ZipCode,fld_DateOfBirth,fld_PhoneNumber,fld_Email)" +
                        "VALUES (@username,@password,@firstName,@lastName,@gender,@address,@zipCode,@dob,@phoneNumber,@email)";
                    MySqlCommand cmd = new MySqlCommand(commandString, conn);

                    cmd.Parameters.AddWithValue("@username", user.UserName);
                    var encryptPass = System.Text.Encoding.UTF8.GetBytes(user.Password);
                    string encryptedPassword = System.Convert.ToBase64String(encryptPass);
                    cmd.Parameters.AddWithValue("@password", encryptedPassword);
                    cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@gender", user.GenderValue);
                    cmd.Parameters.AddWithValue("@address", user.Adres);
                    cmd.Parameters.AddWithValue("@zipCode", user.ZipCode);
                    cmd.Parameters.AddWithValue("@dob", user.DOB);
                    cmd.Parameters.AddWithValue("@phoneNumber", user.Phone);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex )
            {
                WriteErrorLog("Error: " + ex.Message + " - " + DateTime.Now.ToString() + "\r\n" +
                    "fld_UserName as " + user.UserName + "\r\n" + 
                    " fld_Password as " + user.Password + "\r\n" +  
                    " fld_FirstName as " + user.FirstName + "\r\n" + 
                    " fld_LastName as " + user.LastName + "\r\n" + 
                    " fld_Gender as " + user.GenderValue + "\r\n" + 
                    " fld_Address as " + user.Adres + "\r\n" +
                    " fld_ZipCode as " + user.ZipCode + "\r\n" + 
                    " fld_DateOfBirth as " + user.DOB + "\r\n" + 
                    " fld_PhoneNumber as " + user.Phone + "\r\n" + 
                    " fld_Email as " + user.Email + "\r\n\r\n");
            }
        }

        //Aparte methods voor de twee nieuwe procedures

        public int CheckIfUserExists(LoginModel loginMdl)
        {
            int result = 10;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand("CheckUserExists", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    MySqlParameter[] parameters = new MySqlParameter[2];
                    parameters[0] = new MySqlParameter("in_emailaddress", MySqlDbType.VarChar);
                    parameters[0].Value = loginMdl.EmailAddress;
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1] = new MySqlParameter("result", MySqlDbType.Int32);
                    parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int firstResult = (int)parameters[1].Value;

                    if (firstResult == 1)
                    {
                        //password matches, login succesfull (MySQL return value 1 = success)
                        WriteErrorLog("One user found: " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                    else
                    {
                        //password incorrect, no login
                        WriteErrorLog("mailadress: " + loginMdl.EmailAddress + "\r\n\r\n" + "No user found: " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error: " + ex.Message + " - " + DateTime.Now.ToString() + "\r\n" +
                "password check result: " + result + "\r\n\r\n");
                return result;
            }

        }

        public int CheckIfPasswordsMatch(LoginModel loginMdl)
        {
            int result = 10;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand("CheckPasswordMatch", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    MySqlParameter[] parameters = new MySqlParameter[3];
                    parameters[0] = new MySqlParameter("in_emailaddress", MySqlDbType.VarChar);
                    parameters[0].Value = loginMdl.EmailAddress;
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1] = new MySqlParameter("in_password", MySqlDbType.VarChar);
                    parameters[1].Value = loginMdl.Password;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2] = new MySqlParameter("result", MySqlDbType.Int32);
                    parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int firstResult = (int)parameters[2].Value;

                    if (firstResult == 1)
                    {
                        //password matches, login succesfull (MySQL return value 1 = success)
                        WriteErrorLog("Passwords match " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                    else
                    {
                        //password incorrect, no login
                        WriteErrorLog("mailadress: " + loginMdl.EmailAddress + "\r\n\r\n" + "Password: " + loginMdl.Password + " No match " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error PasswordCheck: " + ex.Message + " - " + DateTime.Now.ToString() + "\r\n" +
                "password check result: " + result + "\r\n\r\n");
                return result;
            }

        }




        //Method: UserLogin
        //Summary: De procedure LOGIN die wordt aangeroepen combineert 2 procedures: CheckUserExists en CheckPasswordMatch

        public int UserLogin(LoginModel loginMdl)
        {
            int result = 10;
            loginMdl.Password = Base64Encode(loginMdl.Password);
            WriteErrorLog("Password input" + loginMdl.Password + "\r\n\r\n");

            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    MySqlCommand cmd = new MySqlCommand("LOGIN", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    MySqlParameter[] parameters = new MySqlParameter[3];
                    parameters[0] = new MySqlParameter("in_emailaddress", MySqlDbType.VarChar);
                    parameters[0].Value = loginMdl.EmailAddress;
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1] = new MySqlParameter("in_password", MySqlDbType.VarChar);
                    parameters[1].Value = loginMdl.Password;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2] = new MySqlParameter("masterResult", MySqlDbType.Int32);
                    parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    int firstResult = (int)parameters[2].Value;

                    if (firstResult == 1)
                    {
                        //password matches, login succesfull (MySQL return value 1 = success)
                        WriteErrorLog("Passwords match " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                    else
                    {
                        //password incorrect, no login
                        WriteErrorLog("mailadress: " + loginMdl.EmailAddress + "\r\n\r\n" + "Password: " + loginMdl.Password + " No match " + firstResult + "\r\n\r\n");
                        return firstResult;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error PasswordCheck: " + ex.Message + " - " + DateTime.Now.ToString() + "\r\n" +
                "password check result: " + result + "\r\n\r\n");
                return result;
            }
        }

    }
}