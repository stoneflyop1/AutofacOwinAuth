using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.Core.Domain;

namespace AutofacOwinAuth.Core.Service
{
    public interface IUserService
    {
        IList<User> GetAllUsers();

        User GetUserById(int userId);

        User GetUserByEmail(string email);

        User GetUserByUserName(string userName);

        void DeleteUser(User user);

        void InsertUser(User user);

        void UpdateUser(User user);
    }
}
