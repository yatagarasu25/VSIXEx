using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using VSIXEx.Attributes;

namespace VSIXEx
{
	public class VSCTModelEx
	{
	}

	public class VSCTModel
	{
		public Dictionary<Guid, GuidSymbolType> GuidSymbols;
		public Dictionary<Guid, Dictionary<int, EnumNameValuePair<int>>> CommandIDs;
		public Dictionary<Guid, IEnumerable<CommandType>> CommandSets;
		public List<dynamic> keyBindings;

		public VSCTModel(Assembly assembly)
		{
			GuidSymbols = assembly.EnumGuidSymbols().ToDictionary(i => i.Guid);
			CommandSets = assembly.EnumCommandSets()
				.SelectMany(ca => ca.EnumCommands())
				.GroupBy(i => i.Attribute.Guid)
				.ToDictionary(i => i.Key, i => i as IEnumerable<CommandType>);
			CommandIDs = (from id in assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
							group id by id.Attribute.Guid into g
							select new { Guid = g.Key, IDs = g.SelectMany(gid => gid.Type.EnumEnumValues<int>()).ToDictionary(i => i.Value) })
							.ToDictionary(i => i.Guid, i => i.IDs);

			/*
			var methods = from m in from method in (method.Type as Type).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
						  select new
						  {
							  Method = method,
							  CommandExecuteAttribute = method.GetAttribute<CommandExecuteAttribute>(),
							  KeyBindingAttribute = method.GetAttribute<KeyBindingAttribute>()
						  }
						  where m.CommandExecuteAttribute != null && m.KeyBindingAttribute != null
						  select m;
			*/
			/*
			keyBindings = from m in methods
						  select new
						  {

						  }
						  */
		}
	}
}
