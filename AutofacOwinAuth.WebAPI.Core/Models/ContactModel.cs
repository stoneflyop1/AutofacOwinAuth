using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AutofacOwinAuth.WebAPI.Core.Domain;

namespace AutofacOwinAuth.WebAPI.Core.Models
{
    public class ContactModel
    {
        public const string EmailPattern =
            "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$";

        [Required(ErrorMessage="必须填写{0}")]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必须填写{0}")]
        [Display(Name = "邮箱")]
        [RegularExpression(EmailPattern, ErrorMessage = "{0}的格式不正确")]
        public string Email { get; set; }
        [Display(Name = "电话")]
        public string Phone { get; set; }
    }

    public static class ContactExtensions
    {
        public static Contact ToEntity(this ContactModel model)
        {
            if (model == null) return null;
            return new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone
            };
        }

        public static ContactModel ToModel(this Contact contact)
        {
            if (contact == null) return null;
            return new ContactModel
            {
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone
            };
        }
    }
}
