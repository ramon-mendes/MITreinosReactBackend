using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using FILE = System.IO.File;
using MITreinosReact.DAL;

namespace MITreinosReact.Controllers
{
	public class BaseController : Controller
	{
		public MIContext _db { get; set; }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			//_user = Auth.LoadLoggedUser(HttpContext, _db);

			ViewBag.controller = context.RouteData.Values["controller"].ToString();
			ViewBag.action = context.RouteData.Values["action"].ToString();

			ViewBag.svg_sprites = FILE.ReadAllText(Startup.MapPath("~/img/svg/icon-sprites.svg"));
			ViewBag.svg_sprites_manual = FILE.ReadAllText(Startup.MapPath("~/img/svg/icon-sprites-manual.svg"));

#if DEBUG
			ViewBag.indbg = "true";
#else
			ViewBag.indbg = "false";
#endif

			base.OnActionExecuting(context);
		}

		public void Alert(string msg) => TempData["msg-alert"] = msg;
		public void Error(string msg) => TempData["msg-error"] = msg;
		public void Success(string msg) => TempData["msg-success"] = msg;

		public ActionResult JsonData(object data) => Content(JsonConvert.SerializeObject(data), "application/json");
	}
}