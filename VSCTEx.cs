using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;
using VSIXEx.Templates;



namespace VSIXEx
{
	public static class VSCTEx
	{
		public static IEnumerable<dynamic> EnumKeyBindings(this Assembly assembly)
		{
			var guidByName = assembly.EnumGuidSymbols().ToDictionary(i => i.guid);
			foreach (dynamic cs in assembly.EnumCommandSets())
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
					string guid = guidByName[cs.Attribute.CommandSetId].name;
					int id = method.CommandExecuteAttribute.CommandId;
					var key = method.KeyBindingAttribute;
					yield return new
					{
						guid, id, key
					}.ToExpando();
				}
			}
		}

		public static IEnumerable<dynamic> EnumGuidSymbols(this Assembly assembly)
		{
			foreach (dynamic type in assembly.EnumTypesWithAttribute<GuidSymbolsAttribute>())
			{
				foreach (dynamic field in (type.Type as Type).EnumFieldsWithAttribute<GuidSymbolAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
				{
					Guid guid = new Guid(field.Field.GetValue(null));
					yield return new
					{
						guid,
						name = field.Attribute.GetName(field.Field)
					}.ToExpando();
				}
			}
		}

		public static IEnumerable<dynamic> EnumCommandIDs(this Assembly assembly)
		{
			var idSymbols = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>();
			foreach (dynamic symbol in assembly.EnumGuidSymbols())
			{
				yield return new
				{
					symbol.guid,
					symbol.name,
					ids = from id in idSymbols where id.Attribute.Guid == symbol.guid select id
				}.ToExpando();
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
