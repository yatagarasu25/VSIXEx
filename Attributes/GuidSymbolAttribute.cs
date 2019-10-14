using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class GuidSymbolsAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class GuidSymbolAttribute : Attribute
	{
		public string Name;

		public GuidSymbolAttribute(string name = null)
		{
			Name = name;
		}
	}
}
