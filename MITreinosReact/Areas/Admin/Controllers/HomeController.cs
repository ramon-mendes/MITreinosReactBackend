using Microsoft.AspNetCore.Mvc;
using MITreinosReact.Controllers;
using MITreinosReact.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : BaseAuthController
	{
		public HomeController(MIContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
