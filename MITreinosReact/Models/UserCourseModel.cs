using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models
{
	public class UserCourseModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int CourseId { get; set; }
		public int? CurrentLessonId { get; set; }

		public virtual CourseLessonModel CurrentLesson { get; set; }
		public virtual CourseModel Course { get; set; }
		public virtual UserModel User { get; set; }
	}
}
