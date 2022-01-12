using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models.UI
{
	public class UI_Lesson
	{
		public string Title { get; set; }
		public string Hash { get; set; }
		public bool Completed { get; set; }

		public string Prev { get; set; }
		public string Next { get; set; }
	}
}
