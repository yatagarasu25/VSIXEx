using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;
using VSIXEx.Templates;



namespace VSIXEx
{
	public enum MenuType
	{
		Toolbar
	}

	public enum MenuCommandFlag
	{
		DefaultDocked
	}

	public enum ButtonType
	{
		Button
	}

	public struct GuidSymbolType
	{
		public Guid Guid;
		public string Name;
	}

	public struct CommandIDsType
	{
		public Guid Guid;
		public string Name;
		public IEnumerable<EnumNameValuePair<int>> IDs;
	}

	public struct KeyBindingType
	{
		public string Guid;
		public string Id;
		public string Editor;

		public KeyBindingAttribute Attribute;
	}

	public struct CommandParentType
	{
		public string Guid;
		public string Id;
	}

	public struct CommandIconType
	{
		public string Guid;
		public string Id;
	}

	public struct CommandMenuType
	{
		public string Guid;
		public string Id;
		public MenuType Type;
		public MenuCommandFlag CommandFlag;
		public string ButtonText;
		public string CommandName;
	}

	public struct CommandButtonType
	{
		public string Guid;
		public string Id;
		public int Priority;
		public ButtonType Type;
		public CommandParentType Parent;
		public CommandIconType Icon;
		public string ButtonText;
	}

	public struct CommandBitmapType
	{
		public string Guid;
		public string Href;
		public IEnumerable<EnumNameValuePair<int>> IDs;
	}

	public static class VSCTEx
	{
		public static IEnumerable<GuidSymbolType> EnumGuidSymbols(this Assembly assembly)
		{
			foreach (var type in assembly.EnumTypesWithAttribute<GuidSymbolsAttribute>())
			{
				foreach (var field in (type.Type as Type).EnumFieldsWithAttribute<GuidSymbolAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
				{
					yield return new GuidSymbolType
					{
						Guid = new Guid(field.Field.GetValue(null) as string),
						Name = field.Attribute.GetName(field.Field)
					};
				}
			}
		}


		public static string GenerateKeyBindings(this VSCTModel model)
		{
			return Template.TransformToText<VsctKeyBindings>(new { model }.ToExpando());
		}

		public static string GenerateSymbols(this VSCTModel model)
		{
			return Template.TransformToText<VsctSymbols>(new { model }.ToExpando());
		}

		public static string GenerateCommandMenus(this VSCTModel model)
		{
			return Template.TransformToText<VsctCommandsMenus>(new { model }.ToExpando());
		}

		public static string GenerateCommandButtons(this VSCTModel model)
		{
			return Template.TransformToText<VsctCommandsButtons>(new { model }.ToExpando());
		}

		public static string GenerateCommandBitmaps(this VSCTModel model)
		{
			return Template.TransformToText<VsctCommandsBitmaps>(new { model }.ToExpando());
		}
	}
}
