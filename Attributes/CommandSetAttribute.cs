using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandSetAttribute : Attribute
	{
		public Guid CommandSetId;

		public CommandSetAttribute(string commandSetId)
		{
			CommandSetId = new Guid(commandSetId);
		}
	}
}
