using System;
using ForumApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
	public class PostController : Controller
	{
		private readonly ForumAppDbContext _context;

		public PostController(ForumAppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}

