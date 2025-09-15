namespace Controller.Lib;

public static class HotkeyCode {
	// Mouse buttons
	public static readonly string PrimaryMouse = "primarymouse";
	public static readonly string SecondaryMouse = "secondarymouse";
	public static readonly string MiddleMouse = "middlemouse";

	// Movement
	public static readonly string WalkForward = "walkforward";
	public static readonly string WalkBackward = "walkbackward";
	public static readonly string WalkLeft = "walkleft";
	public static readonly string WalkRight = "walkright";
	public static readonly string Sneak = "sneak";
	public static readonly string Sprint = "sprint";
	public static readonly string Jump = "jump";
	public static readonly string SitDown = "sitdown";

	// Modifiers
	public static readonly string Shift = "shift";
	public static readonly string Ctrl = "ctrl";

	// Inventory / UI
	public static readonly string InventoryDialog = "inventorydialog";
	public static readonly string CharacterDialog = "characterdialog";
	public static readonly string DropItem = "dropitem";
	public static readonly string DropItems = "dropitems";
	public static readonly string ToolModeSelect = "toolmodeselect";
	public static readonly string CoordinatesHud = "coordinateshud";
	public static readonly string BlockInfoHud = "blockinfohud";
	public static readonly string BlockInteractionHelp = "blockinteractionhelp";
	public static readonly string EscapeMenuDialog = "escapemenudialog";
	public static readonly string ToggleHud = "togglehud";

	// Camera
	public static readonly string CycleCamera = "cyclecamera";
	public static readonly string ZoomOut = "zoomout";
	public static readonly string ZoomIn = "zoomin";
	public static readonly string ToggleMouseControl = "togglemousecontrol";

	// Chat / commands
	public static readonly string BeginChat = "beginchat";
	public static readonly string BeginClientCommand = "beginclientcommand";
	public static readonly string BeginServerCommand = "beginservercommand";
	public static readonly string ChatDialog = "chatdialog";
	public static readonly string MacroEditor = "macroeditor";

	// Display / screenshots
	public static readonly string ToggleFullscreen = "togglefullscreen";
	public static readonly string Screenshot = "screenshot";
	public static readonly string MegaScreenshot = "megascreenshot";
	public static readonly string FlipHandSlots = "fliphandslots";

	// Hotbar slots
	public static readonly string HotbarSlot1 = "hotbarslot1";
	public static readonly string HotbarSlot2 = "hotbarslot2";
	public static readonly string HotbarSlot3 = "hotbarslot3";
	public static readonly string HotbarSlot4 = "hotbarslot4";
	public static readonly string HotbarSlot5 = "hotbarslot5";
	public static readonly string HotbarSlot6 = "hotbarslot6";
	public static readonly string HotbarSlot7 = "hotbarslot7";
	public static readonly string HotbarSlot8 = "hotbarslot8";
	public static readonly string HotbarSlot9 = "hotbarslot9";
	public static readonly string HotbarSlot10 = "hotbarslot10";

	// Backpack slots
	public static readonly string BackPackSlot1 = "hotbarslot11";
	public static readonly string BackPackSlot2 = "hotbarslot12";
	public static readonly string BackPackSlot3 = "hotbarslot13";
	public static readonly string BackPackSlot4 = "hotbarslot14";

	// Fly / speed
	public static readonly string DecreaseSpeed = "decspeed";
	public static readonly string IncreaseSpeed = "incspeed";
	public static readonly string DecreaseSpeedFraction = "decspeedfrac";
	public static readonly string IncreaseSpeedFraction = "incspeedfrac";
	public static readonly string CycleFlyModes = "cycleflymodes";
	public static readonly string Fly = "fly";

	// Rendering / debug
	public static readonly string RenderMetaBlocks = "rendermetablocks";
	public static readonly string FpsGraph = "fpsgraph";
	public static readonly string DebugScreenGraph = "debugscreenandgraph";
	public static readonly string ReloadWorld = "reloadworld";
	public static readonly string ReloadShaders = "reloadshaders";
	public static readonly string ReloadTextures = "reloadtextures";
	public static readonly string CompactHeap = "compactheap";
	public static readonly string RecomposeAllGuis = "recomposeallguis";
	public static readonly string CycleDialogOutlineModes = "cycledialogoutlines";
	public static readonly string TickProfiler = "tickprofiler";
	public static readonly string PickBlock = "pickblock";
}