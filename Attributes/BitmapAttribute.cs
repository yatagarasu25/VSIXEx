using System;

namespace VSIXEx.Attributes
{
	[AttributeUsage(AttributeTargets.Enum)]
	public class BitmapAttribute : Attribute
	{
		public string Href;

		public BitmapAttribute(string href)
		{
			Href = href;
		}
	}
}
