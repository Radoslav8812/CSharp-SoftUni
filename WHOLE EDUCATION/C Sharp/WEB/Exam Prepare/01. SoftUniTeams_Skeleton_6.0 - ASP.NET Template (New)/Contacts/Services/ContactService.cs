using System;
using Contacts.Data;
using Contacts.Data.Models;
using Contacts.Models;
using Contacts.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactsDbContext context;

        //public async Task<IEnumerable<ContactViewModel>> GetAllAsync()
        //{
            

        //    return await context.Contacts
        //        .Include(x => x.ApplicationUsersContacts)
        //        .Select(x => new ContactViewModel()
        //        {
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            Email = x.Email,
        //            PhoneNumber = x.PhoneNumber,
        //            Address = x.Address,
        //            Website = x.Website,
                    
        //        }).ToListAsync();
        //}
    }
}

