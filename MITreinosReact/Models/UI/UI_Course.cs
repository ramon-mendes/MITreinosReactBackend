using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models.UI
{
	public class UI_Course
	{
		public int Id { get; set; }
		public string Slug { get; set; }
		public string Title { get; set; }
		public string LogoURL { get; set; }
		public string About { get; set; }

		public List<UI_Module> Modules { get; set; }
	}
}
