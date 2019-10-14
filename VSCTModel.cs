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
		public Dictionary<Guid, dynamic> guidSymbols;
		public Dictionary<Guid, Dictionary<int, dynamic>> commandIDs;
		public List<dynamic> keyBindings;

		public VSCTModel(Assembly assembly)
		{
			guidSymbols = assembly.EnumGuidSymbols().ToDictionary(i => (Guid)i.guid);
			commandIDs = (from id in assembly.EnumTypesWithAttribute<IDSymbolsAttribute>()
							group id by (Guid)id.Attribute.Guid into g
							select new { Guid = g.Key, IDs = g.SelectMany(gid => (gid.Type as Type).EnumEnumValues()).ToDictionary(i => (int)i.Value) })
							.ToDictionary(i => i.Guid, i => i.IDs);
		}
	}
}
