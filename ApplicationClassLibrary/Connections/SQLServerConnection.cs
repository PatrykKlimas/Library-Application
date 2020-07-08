using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ApplicationClassLibrary.Connections
{
    public class SQLServerConnection : IDataConnection
    {
        private string connectionString = GlobalSettings.ConnString("Library");
        public void ReturnBook(int bookID)
        {

            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@BookId", bookID);
                    connection.Execute("spBook_Return", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void ExtendReturnDate(int bookID)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@BookId", bookID);
                    connection.Execute("spBookDetails_ExtendReturnDate", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool WasExtended(int bookID)
        {
            bool output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@BookID", bookID);

                    output = connection.Query<bool>("spBookDetails_WasExtended", p, commandType: CommandType.StoredProcedure).ToList().First();

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;


        }
        public void ChangeReturnDate(int bookID)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@BookId", bookID);
                    connection.Execute("spBookDetails_ReturnDateAdd", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DateTime GetDateByBookID(int id)
        {
            DateTime output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@BookID", id);

                    output = connection.Query<DateTime>("spBook_ReturnDate", p, commandType: CommandType.StoredProcedure).ToList().First();

                }
            }
            catch (Exception)
            {
                output = DateTime.MinValue;
            }
            return output;
        }
        public void EditBook(Book book, string Title, string Author, string Publisher, string sType, string Section, string ISBN, string Desctiption)
        {
            int iType;
            int iSection;
            iType = GetTypeByName(sType);
            iSection = GetSectionByName(Section);
            try
            {
                iType = GetTypeByName(sType);
                iSection = GetSectionByName(Section);
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@Id", book.Id);
                    p.Add("@Title", Title);
                    p.Add("@Author", Author);
                    p.Add("@Publisher", Publisher);
                    p.Add("@TypeID", iType);
                    p.Add("@SectionID", iSection);
                    p.Add("@sDescription", Desctiption);
                    p.Add("@ISBN", ISBN);
                    connection.Execute("spBook_Update", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void AddBook(string Title, string Author, string Publisher, string sType, string Section, string ISBN, string Desctiption)
        {
            try
            {
                int iType;
                int iSection;
                iType = GetTypeByName(sType);
                iSection = GetSectionByName(Section);
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@Title", Title);
                    p.Add("@Author", Author);
                    p.Add("@Publisher", Publisher);
                    p.Add("@TypeID", iType);
                    p.Add("@SectionID", iSection);
                    p.Add("@sDescription", Desctiption);
                    p.Add("@ISBN", ISBN);
                    connection.Execute("spBook_Add", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private int GetSectionByName(string name)
        {
            int output = 0;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@name", name);
                    if (connection.Query<int>("spGet_SectionByName", p, commandType: CommandType.StoredProcedure).ToList().Count > 0)
                    {
                        output = connection.Query<int>("spGet_SectionByName", p, commandType: CommandType.StoredProcedure).ToList().First();
                    }
                    else
                    {
                        output = 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (output == 0)
            {
                try
                {
                    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        var p = new DynamicParameters();
                        p.Add("@name", name);
                        p.Add("@id", output);
                        connection.Execute("spCreate_Section", p, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return output;
        }
        private int GetTypeByName(string name)
        {
            int output = 0;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@name", name);
                    if (connection.Query<int>("spGet_TypeByName", p, commandType: CommandType.StoredProcedure).ToList().Count > 0)
                    {
                        output = connection.Query<int>("spGet_TypeByName", p, commandType: CommandType.StoredProcedure).ToList().First();
                    }
                    else
                    {
                        output = 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            if (output == 0)
            {
                try
                {
                    using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                    {
                        var p = new DynamicParameters();
                        p.Add("@name", name);
                        p.Add("@id", output);
                        connection.Execute("spCreate_Type", p, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return output;
        }
        public string GetTypeByBookId(int id)
        {
            string output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@id", id);
                    if (connection.Query<string>("spGet_TypeByBookID", p, commandType: CommandType.StoredProcedure).ToList() != null)
                    {
                        output = connection.Query<string>("spGet_TypeByBookID", p, commandType: CommandType.StoredProcedure).ToList().First();
                    }
                    else
                    {
                        output = string.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;
        }

        public string GetSectionByBookId(int id)
        {
            string output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@id", id);
                    if (connection.Query<string>("spGet_SectionByBookID", p, commandType: CommandType.StoredProcedure).ToList() != null)
                    {
                        output = connection.Query<string>("spGet_SectionByBookID", p, commandType: CommandType.StoredProcedure).ToList().First();
                    }
                    else
                    {
                        output = string.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;
        }
        public void DeleteBook(int bookId)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@Id", bookId);
                    connection.Execute("spBook_Delete", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<User> SearchForUser(string FName, string LName)
        {
            List<User> output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@FirstName", FName);
                    p.Add("@LastName", LName);
                    output = connection.Query<User>("spUsers_Search", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;
        }
        public void UpdatePassword(string password, int id)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@spassword", password);
                    p.Add("@id", id);
                    connection.Execute("spUsers_UpdatePassword", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void UpdateUserData(string FirstName, string LastName, string sEmail, int id)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@FirstName", FirstName);
                    p.Add("@LastName", LastName);
                    p.Add("@Email", sEmail);
                    p.Add("@id", id);
                    connection.Execute("spUsers_Update", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void BorrowBook(int UserID, int BookID)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", UserID);
                    p.Add("@BookID", BookID);
                    connection.Execute("spBooks_Borrow", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Book> GetSearchBooks(string sAutor, string sTitle)
        {
            List<Book> output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@Author", sAutor);
                    p.Add("@Title", sTitle);
                    output = connection.Query<Book>("spBooks_ByAuthAndTitle", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;
        }
        public List<Book> GetBooksByUserId(int id)
        {

            List<Book> output;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@UserID", id);
                    output = connection.Query<Book>("dbo.spBooks_GetByUserID", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return output;
        }


        public List<Book> GetBooks_All()
        {

            throw new NotImplementedException();
        }

        public User GetUser(string login, string password)
        {
            User user;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@login", login);
                    if (connection.Query<User>("dbo.spUsers_GetByLogin", p, commandType: CommandType.StoredProcedure) != null)
                    {
                        user = connection.Query<User>("dbo.spUsers_GetByLogin", p, commandType: CommandType.StoredProcedure).ToList().First();
                        if (!CryptographyProcessor.Verify(password, user.spassword))
                        {
                            user = null;
                        }
                    }
                    else
                    {
                        user = null;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return user;

        }

        public bool InsertUser(User user)
        {
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@FirstName", user.FirstName);
                    p.Add("@LastName", user.LastName);
                    p.Add("@@Login", user.LoginStr);
                    p.Add("@Email", user.Email);
                    p.Add("@Password", user.spassword);
                    p.Add("@bAdmin", user.bAdmin);

                    connection.Execute("dbo.spUser_Insert", p, commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<User> UsersGetAll()
        {
            List<User> users;
            try
            {
                using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    users = connection.Query<User>("dbo.spUsers_GetAll").ToList();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return users;
        }


    }
}
