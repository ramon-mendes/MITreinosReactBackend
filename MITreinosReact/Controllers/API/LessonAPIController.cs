using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using MITreinosReact.DAL;
using MITreinosReact.Models;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MITreinosReact.Controllers.API
{
	public class LessonAPIController : BaseAuthAPIController
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<LessonAPIController> _logger;

		public LessonAPIController(MIContext db, IServiceScopeFactory scopeFactory, ILogger<LessonAPIController> logger)
		{
			_db = db;
			_scopeFactory = scopeFactory;
            _logger = logger;
		}

		private UserLessonWatchModel GetWatchModel(int lessonId, int userId)
		{
			var watchmodel = _db.UserWatchs.SingleOrDefault(w => w.LessonId == lessonId && w.UserId == userId);
			if(watchmodel == null)
			{
				watchmodel = new UserLessonWatchModel()
				{
					LessonId = lessonId,
					UserId = userId,
				};
				_db.UserWatchs.Add(watchmodel);
				_db.SaveChanges();
			}
			return watchmodel;
		}

		[HttpGet]
		public IActionResult GetForHash([Required] string hash)
		{
			var lesson = _db.CourseLessons.SingleOrDefault(l => l.URLhash == hash);
			if(lesson == null)
				return NotFound();
			var prev = _db.CourseLessons.SingleOrDefault(l => l.ModuleId == lesson.ModuleId && l.Order == lesson.Order - 1);
			var next = _db.CourseLessons.SingleOrDefault(l => l.ModuleId == lesson.ModuleId && l.Order == lesson.Order + 1);

			var usercourse = _db.UserCourse.Single(uc => uc.UserId == UserLogged.Id && uc.CourseId == lesson.Module.CourseId);
			usercourse.CurrentLessonId = lesson.Id;
			_db.SaveChanges();

			var watchmodel = GetWatchModel(lesson.Id, UserLogged.Id);
			return JsonData(new
			{
				Title = lesson.Title,
				VideoPath = lesson.VideoPath,
				Watched = watchmodel.Watched,
				Stars = watchmodel.Stars,
				Note = watchmodel.Note,
				ModuleSlug = lesson.Module.Slug,
				PrevLessonHash = prev?.URLhash,
				NextLessonHash = next?.URLhash,
				PrevLessonTitle = prev?.Title,
				NextLessonTitle = next?.Title,
				Downloads = lesson.Downloads.Select(d => new
				{
					d.Url,
					d.Filename,
					d.Extension,
					d.Size,
				})
			});
		}
		
		[HttpGet]
		public IActionResult SetWatched([Required] string hash, bool watched)
		{
			var lesson = _db.CourseLessons.SingleOrDefault(l => l.URLhash == hash);
			if(lesson == null)
				return NotFound();
			var watchmodel = GetWatchModel(lesson.Id, UserLogged.Id);
			watchmodel.Watched = watched;
			watchmodel.DtWatched = DateTime.Now;
			_db.SaveChanges();
			return Ok();
		}

		[HttpGet]
		public IActionResult SetStars([Required] string hash, int stars)
		{
			var lesson = _db.CourseLessons.SingleOrDefault(l => l.URLhash == hash);
			if(lesson == null)
				return NotFound();
			var watchmodel = GetWatchModel(lesson.Id, UserLogged.Id);
			watchmodel.Stars = stars;
			_db.SaveChanges();
			return Ok();
		}

		[HttpGet]
		public IActionResult SetNote([Required] string hash, string note)
		{
			var lesson = _db.CourseLessons.SingleOrDefault(l => l.URLhash == hash);
			if(lesson == null)
				return NotFound();
			var watchmodel = GetWatchModel(lesson.Id, UserLogged.Id);
			watchmodel.Note = note;
			_db.SaveChanges();
			return Ok();
		}

		private async Task<string> SaveLessonMP3(string hash)
        {
            try
            {
				_logger.LogInformation("SaveLessonMP3: " + hash);

				using(var scope = _scopeFactory.CreateScope())
				{
					string outputPath = Startup.MapPath("App_Data/mp3/" + hash + ".mp3");

					var db = scope.ServiceProvider.GetRequiredService<MIContext>();
					var lesson = db.CourseLessons.Single(l => l.URLhash == hash);

					if(!System.IO.File.Exists(outputPath))
					{
						using(HttpClient client = new HttpClient())
						{
							client.Timeout = TimeSpan.FromMinutes(5);
							var video = await client.GetAsync(lesson.VideoPath);
							video.EnsureSuccessStatusCode();
							var data = await video.Content.ReadAsByteArrayAsync();
							string tmpcache = Startup.MapPath("App_Data\\mp3\\" + hash + ".mp4");
							System.IO.File.WriteAllBytes(tmpcache, data);

							FFmpeg.SetExecutablesPath(Environment.CurrentDirectory);
							IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(tmpcache);

							IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
								?.SetCodec(AudioCodec.mp3);

							await FFmpeg.Conversions.New()
								.AddStream(audioStream)
								.SetOutput(outputPath)
								.Start();

							System.IO.File.Delete(tmpcache);
						}
					}

					return outputPath.Replace('\\', '/');
				}
				_logger.LogInformation("Done processing lesson: " + hash);
			}
			catch(Exception ex)
            {
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		[HttpGet]
		public IActionResult LoadCourseMP3([Required] string slug)
        {
			var course = _db.Courses.Single(c => c.Slug == slug);
			var lessons = course.Lessons.ToList();
			Task.Run(() =>
			{
                foreach(var lesson in lessons)
                {
					SaveLessonMP3(lesson.Value.URLhash).Wait();
				}
				//Parallel.ForEach(lessons, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, (lesson) =>
				//{
				//	SaveLessonMP3(lesson.Value.URLhash).Wait();
				//});
			});
			
			return Ok();
        }

		[HttpGet]
		public async Task<IActionResult> GetMP3NoAuth([Required] string hash)
		{
			var lesson = _db.CourseLessons.Single(l => l.URLhash == hash);
			string output = await SaveLessonMP3(hash);
			bool exists = System.IO.File.Exists(output);
			byte[] data = System.IO.File.ReadAllBytes(output);
			return File(data, "audio/mpeg", Path.GetFileName(lesson.VideoPath));
		}
	}
}
