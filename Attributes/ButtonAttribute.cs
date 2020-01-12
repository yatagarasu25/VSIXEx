using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ButtonAttribute : Attribute
	{
		private int Priority;
		private int Parent;
		private int Icon;
		private string ButtonText;

		public ButtonAttribute(int priority, int parent, int icon, string buttonText)
		{
			Priority = priority;
			Parent = parent;
			Icon = icon;
			ButtonText = buttonText;
		}
	}
}
