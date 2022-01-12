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
using shortid;

namespace MITreinosReact.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class LessonController : BaseCRUDController<CourseLessonModel>
	{
		public LessonController(MIContext db)
		{
			_db = db;
			ShortId.SetCharacters("abcdefghijklmnopqrstuvxwyz0123456789");
		}

		// GET: /Admin/Lesson/List
		public IActionResult List(int id)
		{
			return View(_db.CourseModules.Find(id));
		}

		// GET: /Admin/Lesson/Add
		public IActionResult Add(int id)
		{
			return RetAddView(new CourseLessonModel()
			{
				ModuleId = id,
				Module = new CourseModuleModel()
				{
					CourseId = _db.CourseModules.Find(id).CourseId
				}
			});
		}

		// POST: /Admin/Lesson/Add
		[HttpPost]
		public IActionResult Add(CourseLessonModel model)
		{
			model.Id = 0;

			while(true)
			{
				model.URLhash = ShortId.Generate(new shortid.Configuration.GenerationOptions()
				{
					Length = 10,
					UseNumbers = true,
					UseSpecialCharacters = false
				});
				if(_db.CourseLessons.Any(l => l.URLhash == model.URLhash))
					continue;
				break;
			}

			var res = new CourseLessonValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			_db.CourseLessons.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List), new { id = model.ModuleId });
		}

		// GET: /Admin/Lesson/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.CourseLessons.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/Lesson/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(CourseLessonModel model)
		{
			model = UpdateModel(model.Id, entry =>
			{
				entry.Title = model.Title;
				entry.VideoPath = model.VideoPath;
			});

			return RedirectToAction(nameof(List), new { id = model.ModuleId });
		}

		// GET: /Admin/Lesson/Delete/{ID}
		public IActionResult Delete(int id)
		{
			var model = DeleteModel(id);
			return RedirectToAction(nameof(List), new { id = model.ModuleId });
		}
	}
}
