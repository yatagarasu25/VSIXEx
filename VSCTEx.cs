using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;
using VSIXEx.Templates;



namespace VSIXEx
{
	public struct GuidSymbolType
	{
		public Guid Guid;
		public string Name;
	}

	public struct CommandIDsType
	{
		public Guid Guid;
		public string Name;
		public IEnumerable<TypeAttributePair<IDSymbolsAttribute>> IDs;
	}

	public static class VSCTEx
	{
		public static IEnumerable<dynamic> EnumKeyBindings(this Assembly assembly)
		{
			var guidByName = assembly.EnumGuidSymbols().ToDictionary(i => i.Guid);
			foreach (var cs in assembly.EnumCommandSets())
			{
				var methods = from method in (cs.Type as Type).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
					select new
					{
						Method = method,
						CommandExecuteAttribute = method.GetAttribute<CommandExecuteAttribute>(),
						KeyBindingAttribute = method.GetAttribute<KeyBindingAttribute>()
					};

				foreach (dynamic method in from m in methods
							where m.CommandExecuteAttribute != null && m.KeyBindingAttribute != null
							select m)
				{
					string guid = guidByName[cs.Attribute.Guid].Name;
					int id = method.CommandExecuteAttribute.CommandId;
					var key = method.KeyBindingAttribute;
					yield return new
					{
						guid, id, key
					}.ToExpando();
				}
			}
		}

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

		public static IEnumerable<CommandIDsType> EnumCommandIDs(this Assembly assembly)
		{
			var idSymbols = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>();
			foreach (dynamic symbol in assembly.EnumGuidSymbols())
			{
				Guid symbolGuid = symbol.Guid;
				yield return new CommandIDsType
				{
					Guid = symbol.Guid,
					Name = symbol.Name,
					IDs = idSymbols.Where(id => id.Attribute.Guid == symbolGuid)
				};
			}
		}

		public static string GenerateKeyBindings(this Assembly assembly)
		{
			return Template.TransformToText<VsctKeyBindings>(new
			{
				assembly = assembly
			}.ToExpando());
		}

		public static string GenerateSymbols(this Assembly assembly)
		{
			return Template.TransformToText<VsctSymbols>(new
			{
				assembly = assembly
			}.ToExpando());
		}
	}
}
