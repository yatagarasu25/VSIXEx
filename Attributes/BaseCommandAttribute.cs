using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class BaseCommandAttribute : Attribute
	{
		public int CommandId;

		public BaseCommandAttribute(int commandId)
		{
			CommandId = commandId;
		}
	}
}
