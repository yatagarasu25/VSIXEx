using VSIXEx.Attributes;

namespace VSIXEx
{
	[GuidSymbols]
	public static class VsGuidSymbols
	{
		[GuidSymbol(hidden: true)]
		public const string guidSHLMainMenu = "E02D52D3-67E5-4B79-8B1F-41D2E743A364"; // NOTE: value not correct, but that is not important.
	}

	[IDSymbols(VsGuidSymbols.guidSHLMainMenu, hidden: true)]
	public enum VsGroupIDs
	{
		IDM_VS_CTXT_CODEWIN,
		IDM_VS_CTXT_FOLDERNODE,
		IDM_VS_CTXT_EZDOCWINTAB,
		IDM_VS_CTXT_ITEMNODE,
	}
}
