using System.Reflection;
using SystemEx;
using VSIXEx.Templates;



namespace VSIXEx
{
	public static class VSCTEx
	{
		public static string GenerateSymbols(this Assembly assembly)
		{
			return Template.TransformToText<VsctSymbols>(new
			{
				assembly = assembly
			}.ToExpando());
		}
	}
}
