using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MITreinosReact.Classes;
using MITreinosReact.Controllers;
using MITreinosReact.DAL;
using MITreinosReact.Models;

namespace MITreinosReact.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ModuleController : BaseCRUDController<CourseModuleModel>
	{
		public ModuleController(MIContext db)
		{
			_db = db;
		}

		// GET: /Admin/Module/List
		public IActionResult List(int id)
		{
			return View(_db.Courses.Find(id));
		}

		// GET: /Admin/Module/Add
		public IActionResult Add(int id)
		{
			return RetAddView(new CourseModuleModel() { CourseId = id });
		}

		// POST: /Admin/Module/Add
		[HttpPost]
		public IActionResult Add(CourseModuleModel model)
		{
			var res = new CourseModuleValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			model.Slug = Utils.Slugify(model.Title);

			_db.CourseModules.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List), new { id = model.CourseId});
		}

		// GET: /Admin/Module/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.CourseModules.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/Module/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(CourseModuleModel model)
		{
			model = UpdateModel(model.Id, entry =>
			{
				entry.Title = model.Title;
			});

			return RedirectToAction(nameof(List), new { id = model.CourseId });
		}

		// GET: /Admin/Module/Delete/{ID}
		public IActionResult Delete(int id)
		{
			var model = DeleteModel(id);
			return RedirectToAction(nameof(List), new { id = model.CourseId });
		}
	}
}
