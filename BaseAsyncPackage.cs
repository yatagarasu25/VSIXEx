using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Threading;
using Task = System.Threading.Tasks.Task;



namespace VSIXEx
{
	public class BaseAsyncPackage : AsyncPackage
	{
		public DTE2 DTE2 { get; protected set; }

		public virtual OutputWindowPane ErrorOutputPane { get; } = null;

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			// When initialized asynchronously, the current thread may be a background thread at this point.
			// Do any initialization that requires the UI thread after switching to the UI thread.
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			DTE2 = GetGlobalService(typeof(SDTE)) as DTE2;

			OleMenuCommandService commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				GetType().Assembly.EnumCommandSets().RegisterCommandSets(this, commandService);
			}
		}
	}
}
