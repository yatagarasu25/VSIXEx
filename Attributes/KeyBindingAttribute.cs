using System;
using SystemEx;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	public class KeyBindingAttribute : Attribute
	{
		public string Editor = "guidVSStd97";
		public string Key1;
		public string Mod1;
		public string Key2;
		public string Mod2;
		public string emulator;
		public string Condition;

		public KeyBindingAttribute(string key1, string mod1 = null, string key2 = null, string mod2 = null)
		{
			Key1 = key1;
			Mod1 = mod1;
			Key2 = key2;
			Mod2 = mod2;
		}

		public override string ToString()
		{
			string result = "key1=\"{0}\"".format(Key1);
			if (Mod1 != null) result += " mod1=\"{0}\"".format(Mod1);
			if (Key2 != null) result += " key2=\"{0}\"".format(Key2);
			if (Mod2 != null) result += " mod2=\"{0}\"".format(Mod2);
			return result;
		}
	}
}
