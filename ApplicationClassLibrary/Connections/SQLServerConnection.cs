﻿using System;
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
                     if(connection.Query<User>("dbo.spUsers_GetByLogin", p, commandType: CommandType.StoredProcedure) != null)
                    {
                        user = connection.Query<User>("dbo.spUsers_GetByLogin", p, commandType: CommandType.StoredProcedure).ToList().First();
                        if(!CryptographyProcessor.Verify(password, user.spassword ))
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
