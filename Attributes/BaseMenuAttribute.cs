using System;
using System.Reflection;
using SystemEx;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class BaseMenuAttribute : Attribute
	{
		public FieldInfo Parent;
		public MenuType Type;
		public MenuCommandFlag CommandFlag;
		public string CommandName;
		public string ButtonText;

		public BaseMenuAttribute(object parent, MenuType type, MenuCommandFlag commandFlag, string commandName, string buttonText)
		{
			Parent = (parent as Enum)?.field();
			Type = type;
			CommandFlag = commandFlag;
			CommandName = commandName;
			ButtonText = buttonText;
		}
	}
}
