using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace MITreinosReact.Models
{
    public class CourseLessonDownloadModel
    {
		public int Id { get; set; }
		public int LessonId { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }

		public virtual CourseLessonModel Lesson { get; set; }
    }

	public class CourseLessonDownloadValidator : AbstractValidator<CourseLessonDownloadModel>
	{
		public CourseLessonDownloadValidator()
		{
			RuleFor(l => l.Url).NotEmpty();
		}
	}
}
