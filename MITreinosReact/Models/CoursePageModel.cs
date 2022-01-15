using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace MITreinosReact.Models
{
	public class CoursePageModel
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int Order { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public virtual CourseModel Course { get; set; }
	}

	public class CoursePageValidator : AbstractValidator<CoursePageModel>
	{
		public CoursePageValidator()
		{
			RuleFor(x => x.CourseId).NotEmpty();
			RuleFor(x => x.Order).NotEmpty();
		}
	}
}