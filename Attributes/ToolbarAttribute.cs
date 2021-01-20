using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ToolbarAttribute : BaseMenuAttribute
	{
		public ToolbarAttribute(object parent = null, MenuCommandFlag commandFlag = MenuCommandFlag.None, string commandName = null, string buttonText = null)
			: base(parent, MenuType.Toolbar, commandFlag, commandName, buttonText)
		{
		}
	}
}
