using EnvDTE;
using Microsoft.VisualStudio;
using System.Linq;

namespace VSIXEx
{
	public static class OutputWindowPanesEx
	{
		public static OutputWindowPane BuildOutputPane(this OutputWindowPanes panes)
			=> panes.Cast<OutputWindowPane>()
				.Where(pane => pane.Guid == VSConstants.OutputWindowPaneGuid.BuildOutputPane_string)
				.FirstOrDefault();

		public static OutputWindowPane DebugPane(this OutputWindowPanes panes)
			=> panes.Cast<OutputWindowPane>()
				.Where(pane => pane.Guid == VSConstants.OutputWindowPaneGuid.DebugPane_string)
				.FirstOrDefault();

		public static OutputWindowPane GeneralPane(this OutputWindowPanes panes)
			=> panes.Cast<OutputWindowPane>()
				.Where(pane => pane.Guid == VSConstants.OutputWindowPaneGuid.GeneralPane_string)
				.FirstOrDefault();

		public static OutputWindowPane PaneByName(this OutputWindowPanes panes, string name)
			=> panes.Cast<OutputWindowPane>()
				.Where(pane => pane.Name == name)
				.FirstOrDefault() ?? panes.Add(name);
	}
}
