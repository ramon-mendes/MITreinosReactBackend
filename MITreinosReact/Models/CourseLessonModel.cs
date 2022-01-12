using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace MITreinosReact.Models
{
	public class CourseLessonModel
	{
		public int Id { get; set; }
		public int ModuleId { get; set; }
		public int Order { get; set; }
		public string Title { get; set; }
		public string VideoPath { get; set; }
		public string URLhash { get; set; }
		public virtual CourseModuleModel Module { get; set; }
	}

	public class CourseLessonValidator : AbstractValidator<CourseLessonModel>
	{
		public CourseLessonValidator()
		{
			RuleFor(x => x.ModuleId).NotEmpty();
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.VideoPath).NotEmpty();
			RuleFor(x => x.URLhash).NotEmpty();
		}
	}
}