using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSIXEx
{
	public static class ProjectEx
	{
		public static ProjectItem FindOrAddProjectFolder(this Project project, string path)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			var root = project.ProjectItems;
			foreach (var folder in path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
			{
				var folderItem = root.Cast<ProjectItem>().Where(pi => pi.Name == folder).FirstOrDefault();
				if (folderItem == null)
				{
					root = root.AddFolder(folder).ProjectItems;
				}
				else
				{
					root = folderItem.ProjectItems;
				}
			}

			return root.Parent as ProjectItem;
		}
	}
}
