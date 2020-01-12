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
	public struct CommandType
	{
		public struct KB
		{
			public MethodInfo Method;
			public BaseCommandAttribute Attribute;
			public KeyBindingAttribute KeyBindingAttribute;
		}

		public struct B
		{
			public MethodInfo Method;
			public BaseCommandAttribute Attribute;
			public ButtonAttribute ButtonAttribute;
		}

		public Type Type;
		public CommandSetAttribute Attribute;
		public MethodAttributePair<BaseCommandAttribute> ExecuteCommand;
		public IEnumerable<MethodAttributePair<BaseCommandAttribute>> Commands;
		public IEnumerable<KB> KeyBindings;
		public IEnumerable<B> Buttons;
	}

	public static class PackageEx
	{
		public static IEnumerable<TypeAttributePair<CommandSetAttribute>> EnumCommandSets(this Assembly assembly) => assembly.EnumTypesWithAttribute<CommandSetAttribute>();
		public static IEnumerable<CommandType> EnumCommands(this TypeAttributePair<CommandSetAttribute> commandSet)
		{
			var commandMethodGroups =
				from method in commandSet.Type.EnumMethodsWithAttribute<BaseCommandAttribute>()
				where method.Attribute != null
				group method by method.Attribute.CommandId;

			foreach (var commands in commandMethodGroups)
			{
				yield return new CommandType
				{
					Type = commandSet.Type,
					Attribute = commandSet.Attribute,
					ExecuteCommand = commands.Where(c => c.Attribute is CommandExecuteAttribute).Single(),
					Commands = commands,
					KeyBindings = commands
						.Where(c => c.Attribute is CommandExecuteAttribute)
						.SelectMany(c => c.Method.GetAttributes<KeyBindingAttribute>()
							.Select(a => new CommandType.KB
							{
								Method = c.Method,
								Attribute = c.Attribute,
								KeyBindingAttribute = a
							})),
					Buttons = commands
						.Where(c => c.Attribute is CommandExecuteAttribute)
						.SelectMany(c => c.Method.GetAttributes<ButtonAttribute>()
							.Select(a => new CommandType.B
							{
								Method = c.Method,
								Attribute = c.Attribute,
								ButtonAttribute = a
							})),
				};
			}
		}

		public static void RegisterCommandSets(this IEnumerable<TypeAttributePair<CommandSetAttribute>> commandSets, AsyncPackage package, OleMenuCommandService commandService)
		{
			foreach (var cs in commandSets)
			{
				foreach (var command in cs.EnumCommands())
				{
					var commandSet = 
						Activator.CreateInstance(cs.Type
							, BindingFlags.NonPublic | BindingFlags.Instance, null
							, new object[] { package, commandService }
							, null)
						as BaseCommandSet;

					var menuCommandID = new CommandID(cs.Attribute.Guid, command.ExecuteCommand.Attribute.CommandId);
					var menuCommand = new OleMenuCommand((s, e) => 
						ThreadHelper.JoinableTaskFactory.Run(() =>
							command.ExecuteCommand.Method.Invoke(commandSet, new object[] { (OleMenuCommand)s, e }) as Task)
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
