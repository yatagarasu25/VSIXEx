using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;

namespace VSIXEx
{
	[GuidSymbols]
	public static class VsGuidSymbols
	{
		[GuidSymbol(hidden: true)]
		public const string guidSHLMainMenu = "E02D52D3-67E5-4B79-8B1F-41D2E743A364"; // TODO: value not correct, but that is not important.
	}

	[IDSymbols(VsGuidSymbols.guidSHLMainMenu, hidden: true)]
	public enum VsGroupIDs
	{
		IDM_VS_CTXT_CODEWIN,
		IDM_VS_CTXT_FOLDERNODE,
		IDM_VS_CTXT_EZDOCWINTAB,
		IDM_VS_CTXT_ITEMNODE,
	}

	public static class VsGroupIDs_
	{
		const string s = nameof(VSIXEx) + "." + nameof(VsGroupIDs);
		public const string IDM_VS_CTXT_CODEWIN = s + "." + nameof(VsGroupIDs.IDM_VS_CTXT_CODEWIN) + ", VSIXEx";
		public const string IDM_VS_CTXT_FOLDERNODE = s + "." + nameof(VsGroupIDs.IDM_VS_CTXT_FOLDERNODE) + ", VSIXEx";
		public const string IDM_VS_CTXT_EZDOCWINTAB = s + "." + nameof(VsGroupIDs.IDM_VS_CTXT_EZDOCWINTAB) + ", VSIXEx";
		public const string IDM_VS_CTXT_ITEMNODE = s + "." + nameof(VsGroupIDs.IDM_VS_CTXT_ITEMNODE) + ", VSIXEx";
	}

	public class VSCTModel
	{
		protected Dictionary<Guid, GuidSymbolType> GuidSymbols;
		protected Dictionary<Guid, Dictionary<int, EnumNameValuePair<int>>> CommandIDs;
		protected Dictionary<Guid, IEnumerable<CommandType>> CommandSets;
		protected IEnumerable<CommandGroupType> CommandGroups;
		protected IEnumerable<CommandMenuType> CommandMenus;
		protected IEnumerable<CommandButtonType> CommandButtons;
		protected IEnumerable<CommandBitmapType> CommandBitmaps;

		public VSCTModel(Assembly assembly)
		{
			GuidSymbols = new[] { assembly, typeof(VSCTModel).Assembly }
				.SelectMany(a => a.EnumGuidSymbols())
				.ToDictionary(i => i.Guid);
			CommandIDs = new[] { assembly, typeof(VSCTModel).Assembly }
				.SelectMany(a => a.EnumTypesWithAttribute<IDSymbolsAttribute>())
				.GroupBy(id => id.Attribute.Guid)
				.Select(g => new { Guid = g.Key, IDs = g.SelectMany(gid => gid.Type.EnumEnumValues<int>()).ToDictionary(i => i.Value) })
				.ToDictionary(i => i.Guid, i => i.IDs);
			CommandSets = assembly.EnumCommandSets()
				.SelectMany(ca => ca.EnumCommands())
				.GroupBy(i => i.Attribute.Guid)
				.ToDictionary(i => i.Key, i => i as IEnumerable<CommandType>);
			CommandGroups = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
				.Select(id => new { id.Attribute.Guid, IDs = id.Type.EnumEnumValuesWithAttribute<int, GroupAttribute>() })
				.SelectMany(id => id.IDs.Select(menu => new { id.Guid, Id = menu.Name, Group = menu.Attribute }))
				.Select(id => new CommandGroupType
				{
					Guid = GuidSymbols[id.Guid].Name,
					Id = id.Id,
					Parent = new CommandParentType
					{
						Guid = GuidSymbols[id.Group.Parent.FieldType.GetAttribute<IDSymbolsAttribute>().Guid].Name,
						Id = id.Group.Parent.Name,
					},
					Priority = id.Group.Priority
				});
			CommandMenus = assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
				.Select(id => new { id.Attribute.Guid, IDs = id.Type.EnumEnumValuesWithAttribute<int, BaseMenuAttribute>() })
				.SelectMany(id => id.IDs.Select(menu => new { id.Guid, Id = menu.Name, Menu = menu.Attribute }))
				.Select(id => new CommandMenuType
				{
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
				.Select(id => new CommandBitmapType
				{
					Guid = GuidSymbols[id.Attribute.Guid].Name,
					Href = id.BitmapAttribute.Href,
					IDs = id.Type.EnumEnumValues<int>()
				});
		}

		public IEnumerable<CommandIDsType> EnumCommandIDs(bool withHidden = false)
		{
			return GuidSymbols
				.Where(i => !i.Value.Hidden || withHidden)
				.Select(i => new CommandIDsType
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

		public IEnumerable<CommandGroupType> EnumCommandGroups() => CommandGroups;

		public IEnumerable<CommandButtonType> EnumCommandButtons()
		{
			return CommandSets.SelectMany(i => i.Value.SelectMany(c => c.Buttons)
				.Select(btn => new CommandButtonType
				{
					Guid = GuidSymbols[i.Key].Name,
					Id = CommandIDs[i.Key][btn.Attribute.CommandId].Name,
					Type = btn.ButtonAttribute.Type,
					Priority = btn.ButtonAttribute.Priority,
					Parent = new CommandParentType
					{
						Guid = GuidSymbols[btn.ButtonAttribute.Parent.FieldType.GetAttribute<IDSymbolsAttribute>().Guid].Name,
						Id = btn.ButtonAttribute.Parent.Name,
					},
					Icon = new CommandIconType
					{
						Guid = GuidSymbols[btn.ButtonAttribute.Icon.FieldType.GetAttribute<IDSymbolsAttribute>().Guid].Name,
						Id = btn.ButtonAttribute.Icon.Name,
					},
					ButtonText = btn.ButtonAttribute.ButtonText
				}));
		}

		public IEnumerable<CommandMenuType> EnumCommandMenus() => CommandMenus;
		public IEnumerable<CommandBitmapType> EnumCommandBitmaps() => CommandBitmaps;
	}
}
