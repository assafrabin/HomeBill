using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace BillsProject
{
    static public class DB_API
    {
        private const string server = "localhost";
        private const string database = "HomeBill";
        private const string uid = "root";
        private const string password = "";
        private static string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        private static MySqlConnection connection = new MySqlConnection(connectionString);

        private static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public static void Insert(string username, string password, string email, int notification)
        {
            string query = "INSERT INTO Users (username,password,email,notification) VALUES('" + username + "', '" + password + "', '" + email + "', '" + notification + "')";

            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        public static void Update(string username, string password, string email, int notification, string usernameToUpdate)
        {
            string query = "UPDATE Users SET username='" + username + "', email='" + email + "' WHERE username='" + usernameToUpdate + "'";

            //Open connection
            if (OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        //Delete statement
        public static void Delete(string username)
        {
            string query = "DELETE FROM Users WHERE username='" + username + "'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static string GetPassword(string userName)
        {
            string query = "SELECT password FROM Users WHERE username='" + userName + "'";

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();

                string password = dataReader["password"].ToString();


                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return password;
            }
            else
            {
                return "ERROR: Coudn't connect to the SQL server";
            }
        }

        public static List<string>[] Select()
        {
            string query = "SELECT * FROM Users";

            //Create a list to store the result
            List<string>[] list = new List<string>[4];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["username"] + "");
                    list[1].Add(dataReader["password"] + "");
                    list[2].Add(dataReader["email"] + "");
                    list[3].Add(dataReader["notification"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public static int Count(string userName = "*")
        {
            string query;
            if (userName == "*")
                query = "SELECT Count(*) FROM Users";
            else
                query = "SELECT Count(username) FROM Users WHERE username ='" + userName + "'";

            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public static void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to backup!");
            }
        }
        //Restore
        public static void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error , unable to Restore!");
            }
        }

        /// <summary>
        /// Determines whether [is user exist] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public static bool IsUserExist(string userName)
        {
            if (DB_API.Count(userName) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Gets a password and return the hash of that string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            byte[] passwordInBytes = Encoding.ASCII.GetBytes(password);                     // Convert the input string to bytes in order to hash
            byte[] sha1data = new SHA1CryptoServiceProvider().ComputeHash(passwordInBytes); // Hashing
            string hashedPassword = new ASCIIEncoding().GetString(sha1data);                // Convert back from bytes to string

            return hashedPassword;
        }
    }

}
