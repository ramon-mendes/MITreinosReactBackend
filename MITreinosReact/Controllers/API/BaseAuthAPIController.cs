using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using FILE = System.IO.File;
using MITreinosReact.DAL;
using MITreinosReact.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace MITreinosReact.Controllers
{
	public class BaseAuthAPIController : BaseController
	{
		static readonly string COOKIE_NAME = "UID-API";
		static readonly TimeSpan LOGIN_TIMEOUT = TimeSpan.FromDays(2);
		public static UserModel UserLogged { get; private set; }

		private static Dictionary<string, Tuple<int, DateTime>> _id2user = new Dictionary<string, Tuple<int, DateTime>>();

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			if(context.RouteData.Values["controller"].ToString() == "UserAPI")
			{
				return;
			}

			if(!LoadLoggedUser())
			{
				context.Result = new UnauthorizedObjectResult("User is unauthorized");
			}
		}

		protected bool LoadLoggedUser()
		{
			if(!IsLogged())
				return false;
			var guid = HttpContext.Request.Cookies[COOKIE_NAME];
			if(!_id2user.ContainsKey(guid))
				return false;
			UserLogged = _db.Users.Find(_id2user[guid].Item1);
			return true;
		}

		protected bool IsLogged()
		{
			var guid = HttpContext.Request.Cookies[COOKIE_NAME];
			if(guid == null)
				return false;

			if(!_id2user.ContainsKey(guid))
				return false;
			var login = _id2user[guid];
			if(login.Item2 < DateTime.Now)
				return false;
			return true;
		}

		protected bool AuthLogin(string email, string pwd)
		{
			if(email == null || pwd == null)
				return false;
			email = email.Trim();

			{
				var user = _db.Users.SingleOrDefault(u => u.Email == email && u.PWD == pwd);
				if(user == null)
					return false;

				var lid = Guid.NewGuid().ToString();
				var dt_expires = DateTime.Now.Add(LOGIN_TIMEOUT);
				_id2user[lid] = new Tuple<int, DateTime>(user.Id, dt_expires);
				HttpContext.Response.Cookies.Append(COOKIE_NAME, lid, new CookieOptions() { Expires = new DateTimeOffset(dt_expires) });
				return true;
			}
		}

		protected void Logout()
		{
			var guid = HttpContext.Request.Cookies[COOKIE_NAME];

			HttpContext.Response.Cookies.Delete(COOKIE_NAME);
			if(guid != null)
				_id2user.Remove(guid);
		}
	}
}