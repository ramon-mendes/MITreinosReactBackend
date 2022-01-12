using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MITreinosReact.Controllers;
using MITreinosReact.DAL;
using MITreinosReact.Models;

namespace MITreinosReact.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AuthController : BaseAuthController
	{
		public AuthController(MIContext db)
		{
			_db = db;
		}

		public IActionResult Login(string ReturnUrl)
		{
			return View(new UserManagerModel() { ReturnUrl = ReturnUrl });
		}

		[HttpPost]
		public IActionResult Login(UserManagerModel model)
		{
			if(AuthLogin(model.Email, model.PWD))
			{
				if(model.ReturnUrl != null)
					return Redirect(model.ReturnUrl);
				return Redirect("/Admin");
			}

			var res = new UserManagerValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return View(model);
			}

			return View(model);
		}

		public IActionResult Logout()
		{
			AuthLogout();
			return RedirectToAction(nameof(Login));
		}
	}
}
