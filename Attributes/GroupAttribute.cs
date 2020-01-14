using System;
using System.Reflection;
using SystemEx;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class GroupAttribute : Attribute
	{
		public FieldInfo Parent;
		public int Priority;

		public GroupAttribute(string parent, int priority = -1)
		{
			Parent = parent?.field();
			Priority = priority;
		}
	}
}
