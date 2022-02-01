using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITreinosReact.Classes
{
	public static class HtmlHelpers
	{
		public static object PrintIf(this IHtmlHelper helper, bool cond, string html, string elsehtml = null)
		{
			if(cond)
				return helper.Raw(html);
			if(elsehtml != null)
				return helper.Raw(elsehtml);
			return null;
		}

		public static object PrintIf(this IHtmlHelper helper, bool cond, Func<string> html, Func<string> elsehtml = null)
		{
			if(cond)
				return helper.Raw(html());
			if(elsehtml != null)
				return helper.Raw(elsehtml());
			return null;
		}

		public static IHtmlContent Breadcrumb(this IHtmlHelper helper,
			string current,
			string prev_name1 = null, string prev_url1 = null,
			string prev_name2 = null, string prev_url2 = null,
			string prev_name3 = null, string prev_url3 = null,
			string prev_name4 = null, string prev_url4 = null
		)
		{
			var list = new List<(string Name, string Url)>();
			if(prev_name4 != null)
				list.Add((prev_name4, prev_url4));
			if(prev_name3 != null)
				list.Add((prev_name3, prev_url3));
			if(prev_name2 != null)
				list.Add((prev_name2, prev_url2));
			if(prev_name1 != null)
				list.Add((prev_name1, prev_url1));
			list.Add((current, null));

			return helper.Partial("_Breadcrumb", list);
		}
	}
}