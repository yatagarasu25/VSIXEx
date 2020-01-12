using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ToolbarAttribute : BaseMenuAttribute
	{
		public ToolbarAttribute(MenuCommandFlag commandFlag, string commandName = null, string buttonText = null)
			: base(MenuType.Toolbar, commandFlag, commandName, buttonText)
		{
		}
	}
}
