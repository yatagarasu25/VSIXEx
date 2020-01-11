using Microsoft.VisualStudio.Shell;
using System;



namespace VSIXEx
{
	public class BaseCommandSet
	{
	}

	public class BaseCommandSet<TPackage> : BaseCommandSet
		where TPackage: AsyncPackage
	{
		protected readonly TPackage package;
		protected static BaseCommandSet<TPackage> instance;


		protected BaseCommandSet(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package as TPackage ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			instance = this;
		}
	}
}
