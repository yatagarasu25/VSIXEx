using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class IDSymbolAttribute : Attribute
	{
		public Guid Guid;

		public IDSymbolAttribute(string guid)
		{
			Guid = new Guid(guid);
		}
	}
}
