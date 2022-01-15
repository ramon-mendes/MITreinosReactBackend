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
    public class LessonDownloadController : BaseCRUDController<CourseLessonDownloadModel>
    {
		public LessonDownloadController(MIContext db)
		{
			_db = db;
		}

		// GET: /Admin/LessonDownload/List
		public IActionResult List(int id)
		{
			return View(_db.CourseLessons.Find(id));
		}

		// GET: /Admin/LessonDownload/Add
		public IActionResult Add(int id)
		{
			var lesson = _db.CourseLessons.Find(id);

			return RetAddView(new CourseLessonDownloadModel()
			{
				LessonId = id,
				Lesson = new CourseLessonModel()
                {
					ModuleId = lesson.ModuleId,
					Module = new CourseModuleModel()
                    {
						CourseId = lesson.Module.CourseId
					}
                }
			});
		}

		// POST: /Admin/LessonDownload/Add
		[HttpPost]
		public IActionResult Add(CourseLessonDownloadModel model)
		{
			model.Id = 0;

			var res = new CourseLessonDownloadValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			_db.CourseLessonDownloads.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List), new { id = model.LessonId });
		}

		// GET: /Admin/LessonDownload/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.CourseLessonDownloads.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/LessonDownload/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(CourseLessonDownloadModel model)
		{
			model = UpdateModel(model.Id, entry =>
			{
				entry.Url = model.Url;
			});

			return RedirectToAction(nameof(List), new { id = model.LessonId });
		}

		// GET: /Admin/LessonDownload/Delete/{ID}
		public IActionResult Delete(int id)
		{
			var model = DeleteModel(id);
			return RedirectToAction(nameof(List), new { id = model.LessonId });
		}
	}
}