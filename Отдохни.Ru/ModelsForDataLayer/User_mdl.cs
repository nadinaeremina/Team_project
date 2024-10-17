using Отдохни.Ru.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Отдохни.Ru.ModelsForDataLayer
{
    public class User_mdl
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public User_mdl(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
        public override string ToString()
        {
            return $"{Id,-5} {Login,-25} {Password,-25}";
        }
    }
}
