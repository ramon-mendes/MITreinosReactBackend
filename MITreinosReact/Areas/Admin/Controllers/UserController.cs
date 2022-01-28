using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MITreinosReact.Controllers;
using MITreinosReact.DAL;
using MITreinosReact.Models;
using MITreinosReact.Models.UI;

namespace MITreinosReact.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class UserController : BaseCRUDController<UserModel>
	{
		public UserController(MIContext db)
		{
			_db = db;
		}

		private void SetupSearch(UI_UserSearch search)
		{
			ViewBag.search = search;
		}

		// GET: /Admin/User/List
		[HttpGet]
		public IActionResult List()
		{
			SetupSearch(new UI_UserSearch());
			return View(_db.Users.ToList());
		}

		// POST: /Admin/User/List
		[HttpPost]
		public IActionResult List(UI_UserSearch search)
		{
			var query = _db.Users.AsQueryable();
			if(search.Name != null)
			{
				query = query.Where(u => u.Name.ToLower().Contains(search.Name.ToLower()));
			}
			if(search.Email != null)
			{
				query = query.Where(u => u.Email.ToLower().Contains(search.Email.ToLower()));
			}
			SetupSearch(search);

			return View(query.ToList());
		}

		// GET: /Admin/User/Courses
		public IActionResult Courses(int id)
		{
			var model = _db.Users.Find(id);
			ViewBag.courses = _db.Courses.ToList();
			return View(model);
		}

		// POST: /Admin/User/Courses
		[HttpPost]
		public IActionResult Courses(int id, int[] courses)
		{
			var model = _db.Users.Find(id);
			var usercourses = _db.UserCourse.Where(uc => uc.UserId == id).ToList();
			_db.UserCourse.RemoveRange(usercourses);
			_db.SaveChanges();

			foreach(var item in courses)
			{
				var toadd = new UserCourseModel()
				{
					UserId = id,
					CourseId = item,
				};

				var uc = usercourses.FirstOrDefault(c => c.CourseId == item);
				if(uc!=null)
				{
					toadd.CurrentLessonId = uc.CurrentLessonId;
					toadd.JsonMeta = uc.JsonMeta;
				}
				_db.UserCourse.Add(toadd);
			}

			_db.SaveChanges();

			Success("Cursos editados com sucesso.");
			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/User/Add
		public IActionResult Add()
		{
			return RetAddView(new UserModel()
			{
			});
		}

		// POST: /Admin/User/Add
		[HttpPost]
		public IActionResult Add(UserModel model)
		{
			var res = new UserValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			_db.Users.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/User/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.Users.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/User/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(UserModel model)
		{
			model = UpdateModel(model.Id, entry =>
			{
			});

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/User/Delete/{ID}
		public IActionResult Delete(int id)
		{
			var model = DeleteModel(id);
			return RedirectToAction(nameof(List));
		}
	}
}
