using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.Core.Data;
using AutofacOwinAuth.Core.Domain;

namespace AutofacOwinAuth.Core.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;

        public UserService(IRepository<User> repo)
        {
            _repo = repo;
        }
        public IList<User> GetAllUsers()
        {
            return _repo.Table.ToList();
        }

        public User GetUserById(int userId)
        {
            return _repo.GetById(userId);
        }

        public User GetUserByEmail(string email)
        {
            return _repo.Table.FirstOrDefault(c => c.Email == email);
        }

        public User GetUserByUserName(string userName)
        {
            return _repo.Table.FirstOrDefault(c => c.UserName == userName);
        }

        public void DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            _repo.Delete(user);
        }

        public void InsertUser(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            _repo.Insert(user);
        }

        public void UpdateUser(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            _repo.Update(user);
        }
    }
}
