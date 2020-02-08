using System;
using System.Reflection;

namespace VSIXEx.Attributes
{
	class CommandPlacementAttribute : Attribute
	{
		public FieldInfo Parent;
		public int Priority;


	}
}
