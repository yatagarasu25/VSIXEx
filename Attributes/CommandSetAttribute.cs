using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandSetAttribute : Attribute
	{
		public Guid Guid;

		public CommandSetAttribute(string guid)
		{
			Guid = new Guid(guid);
		}
	}
}
