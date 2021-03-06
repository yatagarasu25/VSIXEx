﻿using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Field)]
	public class MenuAttribute : BaseMenuAttribute
	{
		public MenuAttribute(object parent = null, string commandName = null, string buttonText = null)
			: base(parent, MenuType.Menu, MenuCommandFlag.None, commandName, buttonText)
		{
		}
	}
}
