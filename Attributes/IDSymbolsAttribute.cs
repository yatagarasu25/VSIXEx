using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class IDSymbolsAttribute : Attribute
	{
		public Guid Guid;
		public bool Hidden;

		public IDSymbolsAttribute(string guid, bool hidden = false)
		{
			Guid = new Guid(guid);
			Hidden = hidden;
		}
	}
}
