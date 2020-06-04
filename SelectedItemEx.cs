using EnvDTE;
using System;
using SystemEx;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VSIXEx
{
	public static class SelectedItemEx
	{
		public static Project GetProject(this SelectedItem item)
			=> item.ProjectItem.Collection.ContainingProject;

		public static string GetPathWithinProject(this SelectedItem item)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			List<string> path = new List<string>();
			path.Add(item.Name);

			var parent = item.ProjectItem.GetParent() as ProjectItem;
			while (parent != null)
			{
				path.Insert(0, parent.Name);
				parent = parent.GetParent() as ProjectItem;
			}

			return path.Join(Path.DirectorySeparatorChar);
		}
	}
}
