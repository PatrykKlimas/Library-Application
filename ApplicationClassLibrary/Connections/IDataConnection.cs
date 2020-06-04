using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClassLibrary
{
    public interface IDataConnection
    {
        User GetUser(string login,string password);
        List<Book> GetBooks_All();
        bool InsertUser(User user);
        List<User> UsersGetAll();
        List<Book> GetBooksByUserId(int id);
        List<Book> GetSearchBooks(string sAutor, string sTitle);
        void BorrowBook(int UserID, int BookID);
        void UpdateUserData(string FirstName, string LastName, string sEmail, int id);
        void UpdatePassword(string password, int id);
        List<User> SearchForUser(string FName, string LName);
        void DeleteBook(int bookId);
    }
}
