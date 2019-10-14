using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;
using Task = System.Threading.Tasks.Task;



namespace VSIXEx
{
	public static class PackageEx
	{
		public static IEnumerable<dynamic> EnumCommandSets(this Assembly assembly) => assembly.EnumTypesWithAttribute<CommandSetAttribute>();

		public static void RegisterCommandSets(this IEnumerable<dynamic> commands, AsyncPackage package, OleMenuCommandService commandService)
		{
			foreach (var cs in commands)
			{
				var commandSet = 
					Activator.CreateInstance(cs.Type
						, BindingFlags.NonPublic | BindingFlags.Instance, null
						, new object[] { package, commandService }
						, null)
					as BaseCommand;

				if (commandSet != null)
				{
					var allMethods =
						from method in commandSet.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
						select new {
							Method = method,
							Attribute = method.GetAttribute<BaseCommandAttribute>()
						};

					var commandMethods =
						from method in allMethods
						where method.Attribute != null
						group method by method.Attribute.CommandId;

					foreach (var command in commandMethods)
					{
						var executeMethod =
							(from method in command
							where method.Attribute is CommandExecuteAttribute
							select method).SingleOrDefault();

						if (executeMethod != null)
						{
							var menuCommandID = new CommandID(cs.Attribute.CommandSetId, executeMethod.Attribute.CommandId);
							var menuCommand = new OleMenuCommand((s, e) => 
								ThreadHelper.JoinableTaskFactory.Run(() =>
									executeMethod.Method.Invoke(commandSet, new object[] { (OleMenuCommand)s, e }) as Task)
								, menuCommandID);
							/*
							menuCommand.BeforeQueryStatus += MenuCommand_BeforeQueryStatus;
							*/
							commandService.AddCommand(menuCommand);
						}
					}
				}
			}
		}
	}
}
