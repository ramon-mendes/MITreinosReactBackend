using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace MITreinosReact.Models
{
	public class CourseModel
	{
		public int Id { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }
		public string Path { get; set; }
		public string LogoURL { get; set; }
		public string About { get; set; }
		public bool PaymentBased { get; set; }
		public bool DateBased { get; set; }

		public virtual List<CourseModuleModel> Modules { get; set; }

		[JsonIgnore]
		[NotMapped]
		public Dictionary<int, CourseLessonModel> Lessons
		{
			get
			{
				var res = new Dictionary<int, CourseLessonModel>();
				if(Modules != null)
				{
					foreach(var item in Modules)
					{
						foreach(var lesson in item.Lessons)
							res.Add(lesson.Id, lesson);
					}
				}
				return res;
			}
		}
	}

	public class CourseValidator : AbstractValidator<CourseModel>
	{
		public CourseValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
		}
	}
}