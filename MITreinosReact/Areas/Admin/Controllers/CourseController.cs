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
	public class CourseController : BaseCRUDController<CourseModel>
	{
		public CourseController(MIContext db)
		{
			_db = db;
		}

		// GET: /Admin/Course/List
		public IActionResult List()
		{
			return View(_db.Courses.OrderByDescending(c => c.Title).ToList());
		}

		// GET: /Admin/Event/Add
		public IActionResult Add()
		{
			return RetAddView(new CourseModel());
		}

		// POST: /Admin/Event/Add
		[HttpPost]
		public IActionResult Add(CourseModel model)
		{
			var res = new CourseValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			model.Slug = Utils.Slugify(model.Title);

			_db.Courses.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/Event/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.Courses.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/Event/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(CourseModel model)
		{
			UpdateModel(model.Id, entry =>
			{
				entry.Title = model.Title;
			});

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/Event/Delete/{ID}
		public IActionResult Delete(int id)
		{
			var model = DeleteModel(id);
			return RedirectToAction(nameof(List));
		}
	}
}
