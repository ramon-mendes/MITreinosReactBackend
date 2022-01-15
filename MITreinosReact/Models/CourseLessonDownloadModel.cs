using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models
{
    public class CourseLessonDownloadModel
    {
		public int Id { get; set; }
		public int LessonId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

		public virtual CourseLessonModel Lesson { get; set; }
    }
}
