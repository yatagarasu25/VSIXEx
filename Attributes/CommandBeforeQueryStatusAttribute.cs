using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandBeforeQueryStatusAttribute : BaseCommandAttribute
	{
		public CommandBeforeQueryStatusAttribute(int commandId)
			: base(commandId)
		{
		}
	}
}
