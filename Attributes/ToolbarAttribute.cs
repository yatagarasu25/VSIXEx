using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ToolbarAttribute : BaseMenuAttribute
	{
		public ToolbarAttribute(MenuCommandFlag commandFlag)
			: base(MenuType.Toolbar, commandFlag)
		{
		}
	}
}
