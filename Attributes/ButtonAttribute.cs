using System;
using System.Reflection;
using SystemEx;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ButtonAttribute : Attribute
	{
		public ButtonType Type;
		public int Priority;
		public FieldInfo Parent;
		public FieldInfo Icon;
		public string ButtonText;

		public ButtonAttribute(int priority, string parent, string icon, string buttonText)
		{
			Type = ButtonType.Button;
			Priority = priority;
			Parent = parent?.field();
			Icon = icon?.field();
			ButtonText = buttonText;
		}
	}
}
