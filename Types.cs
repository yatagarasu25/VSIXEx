namespace VSIXEx
{
	public enum MenuType
	{
		Context,
		Menu,
		MenuController,
		MenuControllerLatched,
		Toolbar,
		ToolWindowToolbar
	}

	public enum MenuCommandFlag
	{
		None,
		AlwaysCreate,
		DefaultDocked,
		DefaultInvisible,
		DontCache,
		DynamicVisibility,
		IconAndText,
		NoCustomize,
		NotInTBList,
		NoToolbarClose,
		TextChanges,
		TextIsAnchorCommand,
	}

	public enum ButtonType
	{
		Button,
		MenuButton,
		SplitDropDown
	}
}
