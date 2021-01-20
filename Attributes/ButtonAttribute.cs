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

		public ButtonAttribute(int priority, object parent, object icon, string buttonText)
		{
			Type = ButtonType.Button;
			Priority = priority;
			Parent = (parent as Enum)?.field();
			Icon = (icon as Enum)?.field();
			ButtonText = buttonText;
		}
	}
}
