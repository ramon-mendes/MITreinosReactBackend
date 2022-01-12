using System;

namespace MITreinosReact.Models
{
	public class UserLessonWatchModel
	{
		public int Id { get; set; }
		public int LessonId { get; set; }
		public int UserId { get; set; }

		public bool Watched { get; set; }
		public int Stars { get; set; }
		public DateTime DtWatched { get; set; }
		public string Note { get; set; }

		public virtual CourseLessonModel Lesson { get; set; }
		public virtual UserModel User { get; set; }
	}
}