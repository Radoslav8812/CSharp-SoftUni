using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contacts.Controllers
{
    
    public class ContactsController : Controller
    {
        private readonly IContactService contactService;

        public ContactsController(IContactService _contactService)
        {
            contactService = _contactService;
        }


        //[HttpGet]
        //public async Task<IActionResult> All()
        //{
        //    var model = await contactService.GetAllAsync();
        //    return View(model);
        //}
    }
}

