using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models.UI
{
	public class UI_StaticModel
	{
		public UI_Course Course { get; set; }
		public UI_CourseMeta Meta { get; set; }
		public string UserName { get; set; }
	}

	public class UI_CourseMeta
	{
		public int WatchTotal { get; set; }
		public string CurrentModuleSlug { get; set; }
		public string CurrentLessonHash { get; set; }
		public Dictionary<string, bool> WatchedMap { get; set; }
	}
}