using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models.UI
{
	public class UI_Module
	{
		public string Slug { get; set; }
		public string CourseSlug { get; set; }
		public string Title { get; set; }
		public bool Available { get; set; }
		public DateTime AvailableDt { get; set; }

		public List<UI_Lesson> Lessons { get; set; }
	}
}
