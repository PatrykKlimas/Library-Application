using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ApplicationClassLibrary.Connections
{

    public class MySQLConnection : IDataConnection
    {
        private string connectionString = GlobalSettings.ConnString("MySqlLibrary");

        public List<Book> GetBooksByUserId(int id)
        {
            MySqlDataReader myReader;
            List<Book> output = new List<Book>();
            Book book = new Book();
            string query = "SELECT * from Books where userID = \"" + id + "\"";
            myReader = null;
            try
            {
                myReaderexecute(ref myReader, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    book = new Book
                    {
                        Id = myReader.GetInt32(0),
                        Title = myReader.GetString(1),
                        Author = myReader.GetString(2),
                        Publisher = myReader.GetString(3),
                        UserID = myReader.GetInt32(7),
                        ISBN = myReader.GetString(8)
                    };
                    output.Add(book);
                }

            }

            myReaderClose(ref myReader);
            return output;
        
    }

        public List<Book> GetBooks_All()
        {
            throw new NotImplementedException();
        }

        public User GetUser(string login, string password)
        {
            MySqlDataReader myReader;
            User output = new User();
            User user = new User();
            string query = "SELECT id, FirstName, LastName, LoginStr, spassword, Email, bAdmin from users where LoginStr = \"" + login + "\"";
            myReader = null;
            try
            {
                myReaderexecute(ref myReader, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            if (myReader.HasRows)
            {
                myReader.Read();
                if (CryptographyProcessor.Verify(password, myReader.GetString(4)))
                {
                    user = new User
                    {
                        id = myReader.GetInt32(0),
                        FirstName = myReader.GetString(1),
                        LastName = myReader.GetString(2),
                        LoginStr = myReader.GetString(3),
                        spassword = myReader.GetString(4),
                        Email = myReader.GetString(5),
                        Admin = myReader.GetBoolean(6)
                    };
                }
                else
                {
                    user = null;
                }

            }
            else
            {
                user = null;
            }
            myReaderClose(ref myReader);
            return user;
        }

        public bool InsertUser(User user)
        {
            string admin = user.Admin ? "TRUE" : "FALSE";
            string query = "INSERT INTO users(FirstName, LastName, LoginStr, spassword, Email, bAdmin) " +
                                "VALUES (\"" + user.FirstName + "\",\"" + user.LastName + "\",\"" + user.LoginStr + "\",\"" + user.spassword + "\",\"" + user.Email + "\"," + admin + ")";


            try
            {
                MySQLInsert(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<User> UsersGetAll()
        {
            MySqlDataReader myReader;
            List<User> output = new List<User>();
            User user;
            string query = "SELECT id, FirstName, LastName, LoginStr, spassword, Email, bAdmin from users";
            myReader = null;
            try
            {
                myReaderexecute(ref myReader, query);
            }
            catch (Exception e)
            {
                throw e;
            }
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    user = new User
                    {
                        id = myReader.GetInt32(0),
                        FirstName = myReader.GetString(1),
                        LastName = myReader.GetString(2),
                        LoginStr = myReader.GetString(3),
                        spassword = myReader.GetString(4),
                        Email = myReader.GetString(5),
                        Admin = myReader.GetBoolean(6)
                    };
                    output.Add(user);
                }
            }
            myReaderClose(ref myReader);
            return output;
        }
        private void myReaderexecute(ref MySqlDataReader myReader, string squery)
        {

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase;

            commandDatabase = new MySqlCommand(squery, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                myReader = commandDatabase.ExecuteReader();
                commandDatabase = null;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        private void MySQLInsert(string squery)
        {
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = databaseConnection.CreateCommand();

            try
            {
                databaseConnection.Open();
                commandDatabase.CommandText = squery;
                commandDatabase.ExecuteNonQuery();
                commandDatabase.CommandTimeout = 60;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void myReaderClose(ref MySqlDataReader myReader)
        {
            myReader.Close();
            myReader = null;
        }


    }
}
