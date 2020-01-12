using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class BaseMenuAttribute : Attribute
	{
		public MenuType Type;
		public MenuCommandFlag CommandFlag;
		public string CommandName;
		public string ButtonText;

		public BaseMenuAttribute(MenuType type, MenuCommandFlag commandFlag, string commandName, string buttonText)
		{
			Type = type;
			CommandFlag = commandFlag;
			CommandName = commandName;
			ButtonText = buttonText;
		}
	}
}
