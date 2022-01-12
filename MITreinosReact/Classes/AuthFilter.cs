using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MITreinosReact.Areas.Admin.Controllers;
using MITreinosReact.DAL;

namespace MITreinosReact.Classes
{
	class AuthFilter : IAuthorizationFilter
	{
		private MIContext _db;

		public AuthFilter(MIContext db)
		{
			_db = db;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if((string)context.RouteData.Values["area"] == "Admin")
			{
				if((string)context.RouteData.Values["controller"] == "Auth")
					return;

				if(!BaseAuthController.IsLogged(context.HttpContext))
				{
#if false && DEBUG
					AuthManager.Login(context.HttpContext, MCContext.DEFAULT_USER_EMAIL, MCContext.DEFAULT_USER_PWD, _db);
					return;
#endif

					context.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Admin", ReturnUrl = context.HttpContext.Request.Path });
				}
			}
		}
	}
}