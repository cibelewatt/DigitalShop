using LetsShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsShop.Repoitories
{
    public class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>();
            users.Add(new User { Id = 1, Username = "Luiza Trajano", Password = "123funcionario456", Role = "funcionario" });
            users.Add(new User { Id = 2, Username = "Anderson", Password = "321cliente654", Role = "cliente" });
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();
        }
    }
}
