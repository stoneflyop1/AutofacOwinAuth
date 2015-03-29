using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AutofacOwinAuth.WebAPI.Core.Domain;
using AutofacOwinAuth.WebAPI.Core.Services;
using AutofacOwinAuth.WebAPI.Core.Models;

namespace AutofacOwinAuth.WebAPI.Core.Controllers
{
    [Authorize]
    public class ContactsController : ApiController
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public IEnumerable<ContactModel> Get()
        {
            return _service.GetContacts().Select(c=>c.ToModel()).ToList();
        }

        public ContactModel Get(int id)
        {
            var contact = _service.GetById(id);
            return contact.ToModel();
        }

        public ContactModel Get([FromUri]string email)
        {
            var contact = _service.GetByEmail(email);
            return contact.ToModel();
        }

        public async Task<IHttpActionResult> Post(ContactModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var contact = model.ToEntity();

            await Task.Run(()=>_service.InsertContact(contact));

            return CreatedAtRoute("DefaultApi", new { controller = "contacts", id = contact.Id }, contact.ToModel());
        }
    }
}
