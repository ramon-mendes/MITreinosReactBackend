using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace MITreinosReact.Models
{
	public class CourseModuleModel
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }

		[JsonIgnore]
		public virtual CourseModel Course { get; set; }
		public virtual List<CourseLessonModel> Lessons { get; set; }
	}

	public class CourseModuleValidator : AbstractValidator<CourseModuleModel>
	{
		public CourseModuleValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
		}
	}
}