using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class KeyBindingAttribute : Attribute
	{
		public string Key1;
		public string Mod1;
		public string Key2;
		public string Mod2;

		public KeyBindingAttribute(string key1, string mod1 = null, string key2 = null, string mod2 = null)
		{
			Key1 = key1;
			Mod1 = mod1;
			Key2 = key2;
			Mod2 = mod2;
		}
	}
}
