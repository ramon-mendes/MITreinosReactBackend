using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Models.UI
{
	public enum EPageKind
	{
		PAGE,
		LESSON,
	}

	public class UI_Page
	{
		public EPageKind Kind { get; set; }
		public string Page { get; set; }
	}
}
