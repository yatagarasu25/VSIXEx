using Microsoft.VisualStudio.Shell;
using System;



namespace VSIXEx
{
	public class BaseCommand
	{
	}

	public class BaseCommand<TPackage> : BaseCommand
		where TPackage: AsyncPackage
	{
		protected readonly TPackage package;
		protected static BaseCommand<TPackage> instance;


		protected BaseCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package as TPackage ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			instance = this;

			/*
			foreach (CommandIDs commandId in commandIds)
			{
				var menuCommandID = new CommandID(GuidSymbols.MainCommandSet, (int)commandId);
				var menuCommand = new OleMenuCommand((s, e) => ThreadHelper.JoinableTaskFactory.Run(() => ExecuteAsync((OleMenuCommand)s, e)), menuCommandID);
				menuCommand.BeforeQueryStatus += MenuCommand_BeforeQueryStatus;
				commandService.AddCommand(menuCommand);
			}
			*/
		}
	}
}
