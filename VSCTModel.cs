using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;

namespace VSIXEx
{
	public class VSCTModel
	{
		protected Dictionary<Guid, GuidSymbolType> GuidSymbols;
		protected Dictionary<Guid, Dictionary<int, EnumNameValuePair<int>>> CommandIDs;
		protected Dictionary<Guid, IEnumerable<CommandType>> CommandSets;
		protected IEnumerable<CommandMenuType> CommandMenus;
		protected IEnumerable<CommandButtonType> CommandButtons;
		protected IEnumerable<CommandBitmapType> CommandBitmaps;

		public VSCTModel(Assembly assembly)
		{
			GuidSymbols = assembly.EnumGuidSymbols().ToDictionary(i => i.Guid);
			CommandIDs = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
				.GroupBy(id => id.Attribute.Guid)
				.Select(g => new { Guid = g.Key, IDs = g.SelectMany(gid => gid.Type.EnumEnumValues<int>()).ToDictionary(i => i.Value) })
				.ToDictionary(i => i.Guid, i => i.IDs);
			CommandSets = assembly.EnumCommandSets()
				.SelectMany(ca => ca.EnumCommands())
				.GroupBy(i => i.Attribute.Guid)
				.ToDictionary(i => i.Key, i => i as IEnumerable<CommandType>);
			CommandMenus = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
				.Select(id => new { id.Attribute.Guid, IDs = id.Type.EnumEnumValuesWithAttribute<int, BaseMenuAttribute>() })
				.SelectMany(id => id.IDs.Select(menu => new { id.Guid, Id = menu.Name, Menu = menu.Attribute }))
				.Select(id => new CommandMenuType {
					Guid = GuidSymbols[id.Guid].Name,
					Id = id.Id,
					Type = id.Menu.Type,
					CommandFlag = id.Menu.CommandFlag,
					CommandName = id.Menu.CommandName,
					ButtonText = id.Menu.ButtonText,
				});
			CommandBitmaps = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
				.Select(id => new { id.Type, id.Attribute, BitmapAttribute = id.Type.GetAttribute<BitmapAttribute>() })
				.Where(id => id.BitmapAttribute != null)
				.Select(id => new CommandBitmapType { Guid = GuidSymbols[id.Attribute.Guid].Name, Href = id.BitmapAttribute.Href, IDs = id.Type.EnumEnumValues<int>() });
		}

		public IEnumerable<CommandIDsType> EnumCommandIDs()
		{
			return GuidSymbols.Select(i => new CommandIDsType
			{
				Guid = i.Key,
				Name = i.Value.Name,
				IDs = CommandIDs.Where(ei => ei.Key == i.Key).Select(ei => ei.Value).SelectMany(ei => ei.Values)
			});
		}

		public IEnumerable<KeyBindingType> EnumKeyBindings()
		{
			return CommandSets.SelectMany(i => i.Value.SelectMany(c => c.KeyBindings)
				.Select(kb => new KeyBindingType
				{
					Guid = GuidSymbols[i.Key].Name,
					Id = CommandIDs[i.Key][kb.Attribute.CommandId].Name,
					Editor = "guidVSStd97",
					Attribute = kb.KeyBindingAttribute
				}));
		}

		public IEnumerable<CommandMenuType> EnumCommandMenus() => CommandMenus;
		public IEnumerable<CommandBitmapType> EnumCommandBitmaps() => CommandBitmaps;
	}
}
