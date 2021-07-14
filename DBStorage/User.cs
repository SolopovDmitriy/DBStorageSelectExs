using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStorage
{
    class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}; Login: {Login}; Password: {Password}";
        }

    }
}
