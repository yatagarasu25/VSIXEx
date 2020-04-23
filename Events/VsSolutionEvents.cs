using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace VSIXEx.Events
{
	public class VsSolutionEvents : IVsSolutionEvents3, IDisposable
	{
		IVsSolution solution;
		uint cookie;

		Action<int> OnAfterOpenSolutionFn;

		public VsSolutionEvents(IVsSolution solution,
			Action<int> OnAfterOpenSolution = null)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			this.solution = solution;
			solution.AdviseSolutionEvents(this, out cookie);

			OnAfterOpenSolutionFn = OnAfterOpenSolution;
		}

		public void Dispose()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			solution.UnadviseSolutionEvents(cookie);
		}

		public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded) => VSConstants.S_OK;
		public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel) => VSConstants.S_OK;
		public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved) => VSConstants.S_OK;
		public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy) => VSConstants.S_OK;
		public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel) => VSConstants.S_OK;
		public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy) => VSConstants.S_OK;

		public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
		{
			OnAfterOpenSolutionFn?.Invoke(fNewSolution);
			return VSConstants.S_OK;
		}
		public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel) => VSConstants.S_OK;
		public int OnBeforeCloseSolution(object pUnkReserved) => VSConstants.S_OK;
		public int OnAfterCloseSolution(object pUnkReserved) => VSConstants.S_OK;
		public int OnAfterMergeSolution(object pUnkReserved) => VSConstants.S_OK;

		public int OnBeforeOpeningChildren(IVsHierarchy pHierarchy) => VSConstants.S_OK;
		public int OnAfterOpeningChildren(IVsHierarchy pHierarchy) => VSConstants.S_OK;
		public int OnBeforeClosingChildren(IVsHierarchy pHierarchy) => VSConstants.S_OK;
		public int OnAfterClosingChildren(IVsHierarchy pHierarchy) => VSConstants.S_OK;

	}
}
