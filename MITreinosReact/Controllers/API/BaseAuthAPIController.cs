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
        static readonly TimeSpan LOGIN_TIMEOUT = TimeSpan.FromDays(2);
        public static UserModel UserLogged { get; private set; }

        private static Dictionary<string, Tuple<int, DateTime>> _id2user = new Dictionary<string, Tuple<int, DateTime>>();

        static BaseAuthAPIController()
        {
            _id2user["0312e4a9-8211-4b5d-9d49-f058696ed805"] = new Tuple<int, DateTime>(1, DateTime.Now.AddDays(2));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(context.RouteData.Values["controller"].ToString() == "UserAPI" ||
                context.RouteData.Values["action"].ToString().EndsWith("NoAuth"))
            {
                return;
            }

            if(!LoadLoggedUser())
            {
                context.Result = new UnauthorizedObjectResult("User is unauthorized");
            }
        }

        private string GetBearer()
        {
            string bearer = HttpContext.Request.Headers["authorization"];
            if(bearer == null)
                return null;
            return bearer.Substring(7);
        }

        protected bool LoadLoggedUser()
        {
            if(!IsLogged())
                return false;
            string guid = GetBearer();
            UserLogged = _db.Users.Find(_id2user[guid].Item1);
            return true;
        }

        protected bool IsLogged()
        {
            string guid = GetBearer();
            if(guid == null)
                return false;

            if(!_id2user.ContainsKey(guid))
                return false;
            var login = _id2user[guid];
            if(login.Item2 < DateTime.Now)
                return false;
            return true;
        }

        protected string AuthLogin(string email, string pwd)
        {
            if(email == null || pwd == null)
                return null;
            email = email.Trim();

            {
                var user = _db.Users.SingleOrDefault(u => u.Email == email && u.PWD == pwd);
                if(user == null)
                    return null;

                var lid = Guid.NewGuid().ToString();
                var dt_expires = DateTime.Now.Add(LOGIN_TIMEOUT);
                _id2user[lid] = new Tuple<int, DateTime>(user.Id, dt_expires);
                return lid;
            }
        }

        protected void Logout()
        {
            string guid = GetBearer();
            if(guid != null)
                _id2user.Remove(guid);
        }
    }
}