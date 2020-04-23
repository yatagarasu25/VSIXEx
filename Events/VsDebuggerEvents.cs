using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading.Tasks;

namespace VSIXEx.Events
{
	public class VsDebuggerEvents : IVsDebuggerEvents, IDisposable
	{
		private IVsDebugger service;
		private uint cookie = uint.MaxValue;

		public static async Task<VsDebuggerEvents> CreateAsync(AsyncPackage package)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var debugger = await package.GetServiceAsync(typeof(SVsShellDebugger)) as IVsDebugger;
			if (debugger == null)
				return null;

			return new VsDebuggerEvents(debugger);
		}

		public VsDebuggerEvents(IVsDebugger debugger)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			service = debugger;
			service.AdviseDebuggerEvents(this, out cookie);
		}

		#region IDisposable Support
		public void Dispose()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			Dispose(true);
		}

		bool isDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (isDisposed)
				return;

			if (disposing)
			{
				service?.UnadviseDebuggerEvents(cookie);
			}

			isDisposed = true;
		}
		#endregion

		public int OnModeChange(DBGMODE dbgmodeNew)
		{
			return VSConstants.S_OK;
		}
	}
}
