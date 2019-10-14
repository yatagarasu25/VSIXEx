using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class IDSymbolsAttribute : Attribute
	{
		public Guid Guid;

		public IDSymbolsAttribute(string guid)
		{
			Guid = new Guid(guid);
		}
	}
}
