using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandExecuteAttribute : BaseCommandAttribute
	{
		public CommandExecuteAttribute(int commandId)
			: base(commandId)
		{
		}
	}
}
