using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandBeforeQueryStatusAttribute : BaseCommandAttribute
	{
		public CommandBeforeQueryStatusAttribute(object commandId)
			: base((int)commandId)
		{
		}
	}
}
