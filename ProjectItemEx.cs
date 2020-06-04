using EnvDTE;

namespace VSIXEx
{
	public static class ProjectItemEx
	{
		public static object GetParent(this ProjectItem item)
			=> item?.Collection?.Parent;
	}
}
