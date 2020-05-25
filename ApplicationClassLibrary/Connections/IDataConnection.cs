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
    }
}
