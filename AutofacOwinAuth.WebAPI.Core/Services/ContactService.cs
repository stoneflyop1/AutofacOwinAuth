using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.WebAPI.Core.Domain;
using AutofacOwinAuth.WebAPI.Core.Data;

namespace AutofacOwinAuth.WebAPI.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact> _repo;

        public ContactService(IRepository<Contact> repo)
        {
            _repo = repo;
        }

        public ICollection<Contact> GetContacts()
        {
            return _repo.Table.ToList();
        }

        public Contact GetById(int id)
        {
            return _repo.GetById(id);
        }

        public ICollection<Contact> GetByName(string name)
        {
            return _repo.Table.Where(c => c.Name == name).ToList();
        }

        public Contact GetByEmail(string email)
        {
            return _repo.Table.FirstOrDefault(c => c.Email == email);
        }

        public void InsertContact(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            _repo.Insert(contact);
        }

        public void UpdateContact(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            _repo.Update(contact);
        }

        public void DeleteContact(Contact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            _repo.Delete(contact);
        }
    }
}
