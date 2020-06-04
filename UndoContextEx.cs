using EnvDTE;
using System;

namespace VSIXEx
{
	public static class UndoContextEx
	{
		private class ScopedUndoContext : IDisposable
		{
			UndoContext uc = null;

			public ScopedUndoContext(UndoContext context, string Name, bool Strict = false)
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

				if (!context.IsOpen)
				{
					uc = context;
					uc.Open(Name, Strict);
				}
			}

			public void Dispose()
			{
				Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

				if (uc != null)
				{
					uc.Close();
				}
			}
		}

		public static IDisposable Scoped(this UndoContext context, string Name, bool Strict = false)
		{
			return new ScopedUndoContext(context, Name, Strict);
		}
	}
}
