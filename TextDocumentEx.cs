using EnvDTE;

namespace VSIXEx
{
	public static class TextDocumentEx
	{
		public static string GetText(this TextDocument document)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			var p = document.StartPoint.CreateEditPoint();
			return p.GetText(document.EndPoint);
		}
	}
}
