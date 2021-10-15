using System;

namespace VSIXEx
{
	public class EnumCommandException : Exception
	{
		public EnumCommandException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
