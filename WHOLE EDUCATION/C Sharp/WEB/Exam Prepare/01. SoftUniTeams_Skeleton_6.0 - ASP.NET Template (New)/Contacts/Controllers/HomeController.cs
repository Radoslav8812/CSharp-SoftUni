﻿using Contacts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Contacts.Controllers
{
    public class HomeController : Controller
    {
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //    //if (User?.Identity?.IsAuthenticated ?? false)
        //    //{
        //    //    return RedirectToAction("All", "Contacts");
        //    //}
        //    //return View();
        //}
    }
}