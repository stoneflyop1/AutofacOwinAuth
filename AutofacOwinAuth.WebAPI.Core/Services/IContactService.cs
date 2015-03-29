using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.WebAPI.Core.Domain;

namespace AutofacOwinAuth.WebAPI.Core.Services
{
    public interface IContactService
    {
        ICollection<Contact> GetContacts();

        Contact GetById(int id);

        ICollection<Contact> GetByName(string name);

        Contact GetByEmail(string email);

        void InsertContact(Contact contact);

        void UpdateContact(Contact contact);

        void DeleteContact(Contact contact);


    }
}
