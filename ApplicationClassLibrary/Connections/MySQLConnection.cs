using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationClassLibrary.Models;
using MySql.Data.MySqlClient;

namespace ApplicationClassLibrary.Connections
{

    public class MySQLConnection : IDataConnection
    {
        private string connectionString = GlobalSettings.ConnString("MySqlLibrary");
        public void ReturnBook(int bookID)
        {
            string query;
            try
            {
                query = "Delete from BorrowDetails where BookID = " + bookID;
                MySQLInsert(query);
                query = "Update Books set userID = 0 where Id = " + bookID;
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ExtendReturnDate(int bookID)
        {
            string query;
            try
            {
                query = "Update borrowdetails set bExtended = TRUE, returnDate =" + DateTime.Today.AddDays(14) + "where BookID = " + bookID;
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        bool IDataConnection.WasExtended(int bookID)
        {
            MySqlDataReader myReader;
            bool output = false;

            string query = "Select bExtended from BorrowDetails WHERE BookID =  " +bookID;
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
                    output = myReader.GetBoolean(0);
                }
            }
            else
            {
                output = false;
            }

            return output;
        }
        public void ChangeReturnDate(int bookID)
        {
            string query;
            try
            {
                query = "Insert into borrowdetails(BookID, bExtended, returnDate) values (" + bookID + ",FALSE," + DateTime.Today.AddDays(14) + ")";
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DateTime GetDateByBookID(int id)
        {
            MySqlDataReader myReader;
            List<Section> bookSections = new List<Section>();
            DateTime output = new DateTime();

            string query = "SELECT retrunDate from borrowdetails where BookID = " + id;
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
                    output = myReader.GetDateTime(0);
                }
            }
            else
            {
                output = DateTime.MinValue;
            }

            return output;

        }
        public void EditBook(Book book, string Title, string Author, string Publisher, string sType, string Section, string ISBN, string Desctiption)
        {
            List<Section> bookSections = new List<Section>();
            string query;
            int iType;
            int iSection;
            try
            {
                iType = GetTypeByName(sType);
                iSection = GetSectionByName(Section);
                query = "update books set Author =\"" + Author + "\",ISBN=\"" + ISBN + "\", Publisher=\"" + Publisher + "\"," +
                             " sDescription =\"" + Desctiption + "\", SectionID=" + iSection + ",Title=\"" + Title + "\", TypeID = " + iType + "" +
                             " where books.Id =" + book.Id;         
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void AddBook(string Title, string Author, string Publisher, string sType, string Section, string ISBN, string Desctiption)
        {
            List<Section> bookSections = new List<Section>();
            string query;
            try
            {
                int iType;
                int iSection;
                iType = GetTypeByName(sType);
                iSection = GetSectionByName(Section);
                query = "insert into books(Author, ISBN, Publisher, sDescription, SectionID, Title, TypeID) values (" +
                            "\"" + Author + "\"," +
                            "\"" + ISBN + "\"," +
                            "\"" + Publisher + "\"," +
                            "\"" + Desctiption + "\"," +
                              iSection + "," +
                            "\"" + Title + "\"," +
                            iType + ")";
                MySQLInsert(query);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private int GetSectionByName(string name)
        {
            MySqlDataReader myReader;
            List<Section> bookSections = new List<Section>();
            int output;

            string query = "SELECT Id, Section from section";
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
                    bookSections.Add(new Section
                    {
                        Id = myReader.GetInt32(0),
                        Name = myReader.GetString(1)
                    });
                }
            }
            if (bookSections.Exists(x => x.Name.Equals(name)))
            {
                output = bookSections.Where(x => x.Name.Equals(name)).First().Id;
            }
            else
            {
                query = "Insert into section(Id, Section) values (" + (bookSections.OrderBy(x => x.Id).Last().Id + 1) + ", \"" + name + "\")";
                myReader = null;
                try
                {
                    myReaderexecute(ref myReader, query);
                }
                catch (Exception e)
                {
                    throw e;
                }
                output = (bookSections.OrderBy(x => x.Id).Last().Id + 1);
            }
            myReaderClose(ref myReader);
            return output;
        }
        private int GetTypeByName(string name)
        {
            MySqlDataReader myReader;
            List<BookType> bookTypes = new List<BookType>();
            int output;

            string query = "SELECT Id, sType from booktype";
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
                    bookTypes.Add(new BookType { 
                    Id =  myReader.GetInt32(0),
                    Name =  myReader.GetString(1)
                        });
            }
            }
            if (bookTypes.Exists(x => x.Name.Equals(name)))
            {
                output = bookTypes.Where(x => x.Name.Equals(name)).First().Id;
            }
            else{
                query = "Insert into booktype(Id, sType) values (" + (bookTypes.OrderBy(x => x.Id).Last().Id+1) + ", \"" + name + "\")" ;
                myReader = null;
                try
                {
                    myReaderexecute(ref myReader, query);
                }
                catch (Exception e)
                {
                    throw e;
                }
                output = bookTypes.OrderBy(x => x.Id).Last().Id + 1;
            }
            myReaderClose(ref myReader);
            return output;
        }
        public string GetTypeByBookId(int id)
        {
            MySqlDataReader myReader;
            List<string> output = new List<string>();
            string query = "SELECT sType from books right join booktype on booktype.Id = books.TypeID where books.Id = " + id;
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
                    output.Add(myReader.GetString(0));
                }
            }

            myReaderClose(ref myReader);
            return output.Count > 0 ? output.First() : string.Empty;
        }

        public string GetSectionByBookId(int id)
        {
            MySqlDataReader myReader;
            List<string> output = new List<string>();
            string query = "select Section from books right join section on section.Id = books.SectionID where books.id= " + id;
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
                    output.Add(myReader.GetString(0));
                }
            }

            myReaderClose(ref myReader);
            return output.Count > 0 ? output.First() : string.Empty;
        }
        public void DeleteBook(int bookId)
        {
            string query = "Delete from books where Id  = " + bookId;
            try
            {
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<User> SearchForUser(string FName, string LName)
        {
            MySqlDataReader myReader;
            List<User> output = new List<User>();
            User user = new User();
            string query = "SELECT id, FirstName, LastName, LoginStr, spassword, Email, bAdmin from users where FirstName like \"%" + FName + "%\" and LastName like \"%" + LName + "%\"";
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
                        bAdmin = myReader.GetBoolean(6)
                    };
                    output.Add(user);
                }
            }

            myReaderClose(ref myReader);
            return output;
        }
        public void UpdatePassword(string password, int id)
        {
            string query = "UPDATE users SET spassword  = \"" + password + "\"  where Id = " + id;
            try
            {
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void UpdateUserData(string FirstName, string LastName, string sEmail, int id)
        {
            string query = "UPDATE users SET FirstName  = \"" + FirstName + "\", LastName = \"" + LastName + "\", Email =\"" + sEmail + "\"  where Id = " + id;
            try
            {
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void BorrowBook(int UserID, int BookID)
        {
            string query = "UPDATE books SET userID = " + UserID + " where Id =" + BookID;
            try
            {
                MySQLInsert(query);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Book> GetSearchBooks(string sAutor, string sTitle)
        {
            MySqlDataReader myReader;
            List<Book> output = new List<Book>();
            Book book = new Book();
            string query = "SELECT * from Books where Author like \"%" + sAutor + "%\" and Title like \"%" + sTitle + "%\"";
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
                        Type = myReader.GetInt32(4),
                        Section = myReader.GetInt32(5),
                        ISBN = myReader.GetString(8),
                    };
                    
                    try
                    {
                        book.sDescription = myReader.GetString(6) != null ? myReader.GetString(6) : string.Empty;
                    }
                    catch (Exception) { };
                    try
                    {
                        book.Publisher = myReader.GetString(3) != null ? myReader.GetString(3) : string.Empty;
                    }
                    catch (Exception) { };
                    try
                    {
                        book.UserID = myReader.GetInt32(7);
                    }
                    catch (Exception) { };
                    output.Add(book);

                }

            }

            myReaderClose(ref myReader);
            return output;
        }
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
                        bAdmin = myReader.GetBoolean(6)
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
            string admin = user.bAdmin ? "TRUE" : "FALSE";
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
                        bAdmin = myReader.GetBoolean(6)
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
