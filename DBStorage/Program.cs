using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            DbUsersManager dbUsersManager = new DbUsersManager();
            //dbUsersManager.DeleteById(27);
            List<User> users = dbUsersManager.SelectAllUsers();  // сортировать по паролю         
            foreach (User user in users)
            {
                Console.WriteLine(user);
            }

            Console.ReadKey();
        }
    }
}
