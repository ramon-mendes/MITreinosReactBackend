using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MITreinosReact.Classes;
using MITreinosReact.DAL;
using MITreinosReact.Models;
using MITreinosReact.Models.UI;
using Newtonsoft.Json;
using shortid;

namespace MITreinosReact.Controllers.API
{
	public class StaticAPIController : BaseAuthAPIController
	{
		private readonly ILogger<StaticAPIController> _logger;

		public StaticAPIController(MIContext db, ILogger<StaticAPIController> logger)
		{
			_db = db;
			_logger = logger;
		}

		private List<UI_Module> LoadCourseModules(CourseModel course)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			var modules = new List<UI_Module>();
			foreach(var item in course.Modules)
			{
				var lessons = new List<UI_Lesson>();

				Stopwatch sw4 = new Stopwatch();
				var dblessons = item.Lessons.OrderBy(l => l.Order).ToList();
				sw4.Stop();
				_logger.LogError("sw4" + sw4.ElapsedMilliseconds + "ms");

				foreach(var item2 in dblessons)
				{
					Stopwatch sw2 = new Stopwatch();
					sw2.Restart();

					var watch = _db.UserWatchs.FirstOrDefault(uw => uw.LessonId == item.Id && uw.UserId == UserLogged.Id);
					lessons.Add(new UI_Lesson()
					{
						Hash = item2.URLhash,
						Title = item2.Title,
						Completed = watch != null && watch.Watched,
					});

					sw2.Stop();
					_logger.LogError("inner loop took " + sw2.ElapsedMilliseconds + "ms");
				}

				Stopwatch sw3 = new Stopwatch();
				for(int i = 0; i < lessons.Count; i++)
				{
					lessons[i].Prev = i == 0 ? null : lessons[i - 1].Hash;
					lessons[i].Next = i == lessons.Count - 1 ? null : lessons[i + 1].Hash;
				}

				modules.Add(new UI_Module()
				{
					Slug = item.Slug,
					CourseSlug = course.Slug,
					Available = true,
					AvailableDt = DateTime.Now,
					Title = item.Title,
					Lessons = lessons
				});
				_logger.LogError("sw3 " + sw3.ElapsedMilliseconds + "ms");
			}

			sw.Stop();
			_logger.LogError("LoadCourseModules took " + sw.ElapsedMilliseconds + "ms");

			return modules;
		}

		[HttpGet]
		public IActionResult GetForCourse([Required] string slug)
		{
			_logger.LogError("GetForCourse " + slug);
			var course = _db.Courses.SingleOrDefault(c => c.Slug == slug);
			if(course == null)
				return NotFound();

			var usercourse = _db.UserCourse.Single(uc => uc.UserId == UserLogged.Id && uc.CourseId == course.Id);
			if(usercourse.CurrentLessonId == null)
			{
				usercourse.CurrentLessonId = course.Lessons.First().Value.Id;
			}

			var watchmodel = _db.UserWatchs.Where(w => w.Lesson.Module.CourseId == course.Id && w.UserId == UserLogged.Id);
			var currentlesson = _db.CourseLessons.Find(usercourse.CurrentLessonId);

			UI_StaticModel res = new UI_StaticModel()
			{
				UserName = UserLogged.Name,
				Course = new UI_Course()
				{
					Id = course.Id,
					About = course.About,
					LogoURL = course.LogoURL,
					Slug = course.Slug,
					Title = course.Title,
					Modules = LoadCourseModules(course)
				},
				Meta = new UI_CourseMeta()
				{
					WatchTotal = course.Modules.Sum(m => m.Lessons.Count),
					WatchedMap = new Dictionary<string, bool>(),
					CurrentLessonHash = currentlesson.URLhash,
					CurrentModuleSlug = currentlesson.Module.Slug
				},
			};
			foreach(var item in watchmodel)
			{
				res.Meta.WatchedMap[item.Lesson.Module.Course.Slug + "-" + item.Lesson.URLhash] = item.Watched;
			}

			return JsonData(res);
		}

		[HttpGet]
		public IActionResult UserCourses()
		{
			var courses = _db.UserCourse.Where(uc => uc.UserId == UserLogged.Id).Select(uc => new
			{
				Title = uc.Course.Title,
				Slug = uc.Course.Slug,
				LogoURL = uc.Course.LogoURL,
			}).ToList();
			return Json(courses);
		}

		[HttpGet]
		public IActionResult CreateIEData()
		{
			ShortId.SetCharacters("abcdefghijklmnopqrstuvxwyz0123456789");

			int imodule = 1;

			#region Homens
			{
				dynamic modules = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("C:/Users/r.fernandes.mendes/Documents/ProjetosMVC/MITreinosReact/MITreinosReact/App_Data/ie-homens.json"));
				foreach(var module in modules)
				{
					string title = module.title;
					dynamic videos = module.videos;

					_db.CourseModules.Add(new CourseModuleModel()
					{
						Id = imodule,
						CourseId = 1,
						Title = title,
						Slug = Utils.Slugify(title),
					});

					int iclass = 1;
					foreach(var video in videos)
					{
						_db.CourseLessons.Add(new CourseLessonModel()
						{
							Title = "Aula " + iclass++,
							ModuleId = imodule,
							Order = iclass,
							VideoPath = video,
							URLhash = ShortId.Generate(new shortid.Configuration.GenerationOptions()
							{
								Length = 10,
								UseNumbers = true,
								UseSpecialCharacters = false
							}),
						});
					}

					imodule++;
				}
			}
			#endregion

			#region Mulheres
			{
				dynamic modules = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("C:/Users/r.fernandes.mendes/Documents/ProjetosMVC/MITreinosReact/MITreinosReact/App_Data/ie-mulheres.json"));
				foreach(var module in modules)
				{
					string title = module.title;
					dynamic videos = module.videos;

					_db.CourseModules.Add(new CourseModuleModel()
					{
						Id = imodule,
						CourseId = 2,
						Title = title,
						Slug = Utils.Slugify(title),
					});

					int iclass = 1;
					foreach(var video in videos)
					{
						_db.CourseLessons.Add(new CourseLessonModel()
						{
							Title = "Aula " + iclass++,
							ModuleId = imodule,
							Order = iclass,
							VideoPath = video,
							URLhash = ShortId.Generate(new shortid.Configuration.GenerationOptions()
							{
								Length = 10,
								UseNumbers = true,
								UseSpecialCharacters = false
							}),
						});
					}

					imodule++;
				}
			}
			#endregion

			_db.SaveChanges();
			return Ok();
		}
	}
}
