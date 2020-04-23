using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace VSIXEx.Events
{
	public class VsCustomDebuggerEventHandler : IVsCustomDebuggerEventHandler110, Microsoft.VisualStudio.OLE.Interop.IServiceProvider, IDisposable
	{
		private IProfferService profferService;
		private uint cookie;
		private Guid guid;
		private Action<Guid, VsComponentMessage> OnCustomDebugEventFn;

		public static async Task<VsCustomDebuggerEventHandler> CreateAsync(AsyncPackage package, Guid guid
			, Action<Guid, VsComponentMessage> OnCustomDebugEvent = null)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			var profferService = await package.GetServiceAsync(typeof(IProfferService)) as IProfferService;
			if (profferService == null)
				return null;

			return new VsCustomDebuggerEventHandler(profferService, guid, OnCustomDebugEvent);
		}

		public VsCustomDebuggerEventHandler(IProfferService profferService, Guid guid, Action<Guid, VsComponentMessage> OnCustomDebugEvent)
		{
			this.profferService = profferService;
			this.guid = guid;
			OnCustomDebugEventFn = OnCustomDebugEvent;

			profferService.ProfferService(ref guid, this, out cookie);
		}

		#region IDisposable Support
		public void Dispose()
		{
			Dispose(true);
		}

		private bool isDisposed = false; // To detect redundant calls
		protected virtual void Dispose(bool disposing)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			if (isDisposed)
				return;

			if (disposing)
			{
				profferService?.RevokeService(cookie);
			}

			isDisposed = true;
		}
		#endregion

		public int OnCustomDebugEvent(ref Guid ProcessId, VsComponentMessage message)
		{
			OnCustomDebugEventFn?.Invoke(ProcessId, message);
			return VSConstants.S_OK;
		}

		public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			if (guidService == guid)
			{
				if (riid == typeof(IVsCustomDebuggerEventHandler110).GUID)
				{
					ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IVsCustomDebuggerEventHandler110));
					return VSConstants.S_OK;
				}
			}

			ppvObject = IntPtr.Zero;
			return VSConstants.E_NOTIMPL;
		}
	}
}
