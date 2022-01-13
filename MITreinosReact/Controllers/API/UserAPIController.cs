using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MITreinosReact.DAL;

namespace MITreinosReact.Controllers.API
{
	public class UserAPIController : BaseAuthAPIController
	{
		public UserAPIController(MIContext db)
		{
			_db = db;
		}

		public class LoginBody
		{
			public string email { get; set; }
			public string password { get; set; }
		}

		[HttpPost]
		public IActionResult Login([FromBody]LoginBody model)
		{
			string token =  AuthLogin(model.email, model.password);
			return token != null ? Ok(token) : Unauthorized();
		}

		[HttpGet]
		public IActionResult IsAuthorized()
		{
			bool bOK = IsLogged();
			return bOK ? Ok() : Unauthorized();
		}
	}
}
