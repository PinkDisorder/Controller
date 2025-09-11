using JetBrains.Annotations;

namespace Controller.Lib;

public sealed record HotkeyCode {
	public string Value { get; }

	private HotkeyCode(string value) => Value = value;

	public override string ToString() => Value;

	// Mouse buttons
	public static readonly HotkeyCode PrimaryMouse = new("primarymouse");
	public static readonly HotkeyCode SecondaryMouse = new("secondarymouse");
	public static readonly HotkeyCode MiddleMouse = new("middlemouse");

	// Movement
	public static readonly HotkeyCode WalkForward = new("walkforward");
	public static readonly HotkeyCode WalkBackward = new("walkbackward");
	public static readonly HotkeyCode WalkLeft = new("walkleft");
	public static readonly HotkeyCode WalkRight = new("walkright");
	public static readonly HotkeyCode Sneak = new("sneak");
	public static readonly HotkeyCode Sprint = new("sprint");
	public static readonly HotkeyCode Jump = new("jump");
	public static readonly HotkeyCode SitDown = new("sitdown");

	// Modifiers
	public static readonly HotkeyCode Shift = new("shift");
	public static readonly HotkeyCode Ctrl = new("ctrl");

	// Inventory / UI
	public static readonly HotkeyCode InventoryDialog = new("inventorydialog");
	public static readonly HotkeyCode CharacterDialog = new("characterdialog");
	public static readonly HotkeyCode DropItem = new("dropitem");
	public static readonly HotkeyCode DropItems = new("dropitems");
	public static readonly HotkeyCode ToolModeSelect = new("toolmodeselect");
	public static readonly HotkeyCode CoordinatesHud = new("coordinateshud");
	public static readonly HotkeyCode BlockInfoHud = new("blockinfohud");
	public static readonly HotkeyCode BlockInteractionHelp = new("blockinteractionhelp");
	public static readonly HotkeyCode EscapeMenuDialog = new("escapemenudialog");
	public static readonly HotkeyCode ToggleHud = new("togglehud");

	// Camera
	public static readonly HotkeyCode CycleCamera = new("cyclecamera");
	public static readonly HotkeyCode ZoomOut = new("zoomout");
	public static readonly HotkeyCode ZoomIn = new("zoomin");
	public static readonly HotkeyCode ToggleMouseControl = new("togglemousecontrol");

	// Chat / commands
	public static readonly HotkeyCode BeginChat = new("beginchat");
	public static readonly HotkeyCode BeginClientCommand = new("beginclientcommand");
	public static readonly HotkeyCode BeginServerCommand = new("beginservercommand");
	public static readonly HotkeyCode ChatDialog = new("chatdialog");
	public static readonly HotkeyCode MacroEditor = new("macroeditor");

	// Display / screenshots
	public static readonly HotkeyCode ToggleFullscreen = new("togglefullscreen");
	public static readonly HotkeyCode Screenshot = new("screenshot");
	public static readonly HotkeyCode MegaScreenshot = new("megascreenshot");
	public static readonly HotkeyCode FlipHandSlots = new("fliphandslots");

	// Hotbar slots
	public static readonly HotkeyCode HotbarSlot1 = new("hotbarslot1");
	public static readonly HotkeyCode HotbarSlot2 = new("hotbarslot2");
	public static readonly HotkeyCode HotbarSlot3 = new("hotbarslot3");
	public static readonly HotkeyCode HotbarSlot4 = new("hotbarslot4");
	public static readonly HotkeyCode HotbarSlot5 = new("hotbarslot5");
	public static readonly HotkeyCode HotbarSlot6 = new("hotbarslot6");
	public static readonly HotkeyCode HotbarSlot7 = new("hotbarslot7");
	public static readonly HotkeyCode HotbarSlot8 = new("hotbarslot8");
	public static readonly HotkeyCode HotbarSlot9 = new("hotbarslot9");
	public static readonly HotkeyCode HotbarSlot10 = new("hotbarslot10");

	// Backpack slots
	public static readonly HotkeyCode BackPackSlot1 = new("hotbarslot11");
	public static readonly HotkeyCode BackPackSlot2 = new("hotbarslot12");
	public static readonly HotkeyCode BackPackSlot3 = new("hotbarslot13");
	public static readonly HotkeyCode BackPackSlot4 = new("hotbarslot14");

	// Fly / speed
	public static readonly HotkeyCode DecreaseSpeed = new("decspeed");
	public static readonly HotkeyCode IncreaseSpeed = new("incspeed");
	public static readonly HotkeyCode DecreaseSpeedFraction = new("decspeedfrac");
	public static readonly HotkeyCode IncreaseSpeedFraction = new("incspeedfrac");
	public static readonly HotkeyCode CycleFlyModes = new("cycleflymodes");
	public static readonly HotkeyCode Fly = new("fly");

	// Rendering / debug
	public static readonly HotkeyCode RenderMetaBlocks = new("rendermetablocks");
	public static readonly HotkeyCode FpsGraph = new("fpsgraph");
	public static readonly HotkeyCode DebugScreenGraph = new("debugscreenandgraph");
	public static readonly HotkeyCode ReloadWorld = new("reloadworld");
	public static readonly HotkeyCode ReloadShaders = new("reloadshaders");
	public static readonly HotkeyCode ReloadTextures = new("reloadtextures");
	public static readonly HotkeyCode CompactHeap = new("compactheap");
	public static readonly HotkeyCode RecomposeAllGuis = new("recomposeallguis");
	public static readonly HotkeyCode CycleDialogOutlineModes = new("cycledialogoutlines");
	public static readonly HotkeyCode TickProfiler = new("tickprofiler");
	public static readonly HotkeyCode PickBlock = new("pickblock");
}