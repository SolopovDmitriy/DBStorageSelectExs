using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStorage
{
    class DbUsersManager:DbManager
    {
        public DbUsersManager():base("users")
        {
           
        }


        public List<User> SelectAllUsers()
        {
            return SelectAllUsers(null, 0, 100);
        }


        public List<User> SelectAllUsers(int offset, int fetch)
        {
            return SelectAllUsers(null, offset, fetch);
        }


        public List<User> SelectAllUsers(string orderParameter)
        {
            return SelectAllUsers(orderParameter, 0, 100);
        }


        public List<User> SelectAllUsers(string orderParameter, int offset, int fetch)
        {
            List<User> users = new List<User>();// создаем пустой список users - пользователей
            DbDataReader reader = SelectAll(orderParameter, offset, fetch);
            while (reader.Read())
            {
                User user = new User
                {
                    Id = (int)reader["id"],
                    Login = reader["login"].ToString(),
                    Password = reader["password"].ToString()
                };
                users.Add(user);
            }
            Close();
            return users;
        }



    }
}
