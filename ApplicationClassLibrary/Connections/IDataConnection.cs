﻿using System;
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
        string GetTypeByBookId(int id);
        string GetSectionByBookId(int id);
        void AddBook(string Title, string Author, string Publisher,
                       string sType, string Section, string ISBN, string Desctiption);
        void EditBook(Book book ,string Title, string Author, string Publisher,
                       string sType, string Section, string ISBN, string Desctiption);
        DateTime GetDateByBookID(int id);
        void ChangeReturnDate(int bookID);
        bool WasExtended(int bookID);
        void ExtendReturnDate(int bookID);
        void ReturnBook(int bookID);
    }
}
