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
    public class UserController : BaseCRUDController<UserModel>
	{
		public UserController(MIContext db)
		{
			_db = db;
		}

		// GET: /Admin/User/List
		public IActionResult List()
		{
			return View(_db.Users.ToList());
		}

		// GET: /Admin/User/Add
		public IActionResult Add(int id)
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
