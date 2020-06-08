using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandExecuteAttribute : BaseCommandAttribute
	{
		public CommandExecuteAttribute(object commandId)
			: base((int)commandId)
		{
		}
	}
}
