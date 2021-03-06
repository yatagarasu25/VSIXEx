﻿using System;
using System.Reflection;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class GuidSymbolsAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class GuidSymbolAttribute : Attribute
	{
		public string Name;
		public bool Hidden;

		public GuidSymbolAttribute(string name = null, bool hidden = false)
		{
			Name = name;
			Hidden = hidden;
		}

		public string GetName(FieldInfo field)
		{
			return Name != null ? Name : field.Name;
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public class PackageGuidSymbolAttribute : Attribute
	{
	}
}
