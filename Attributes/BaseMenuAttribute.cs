using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class BaseMenuAttribute : Attribute
	{
		public MenuType Type;
		public MenuCommandFlag CommandFlag;

		public BaseMenuAttribute(MenuType type, MenuCommandFlag commandFlag)
		{
			Type = type;
			CommandFlag = commandFlag;
		}
	}
}
