using JetBrains.Annotations;

namespace Controller.Lib;

public static class Hotkeys {
	//"Primary mouse button"
	[UsedImplicitly] public const string PrimaryMouse = "primarymouse";

	//"Second mouse button"
	[UsedImplicitly] public const string SecondaryMouse = "secondarymouse";

	//"Middle mouse button"
	[UsedImplicitly] public const string MiddleMouse = "middlemouse";

	//"Walk forward"
	[UsedImplicitly] public const string WalkForward = "walkforward";

	//"Walk backward"
	[UsedImplicitly] public const string WalkBackward = "walkbackward";

	//"Walk left"
	[UsedImplicitly] public const string WalkLeft = "walkleft";

	//"Walk right"
	[UsedImplicitly] public const string WalkRight = "walkright";

	//"Sneak"
	[UsedImplicitly] public const string Sneak = "sneak";

	//"Sprint"
	[UsedImplicitly] public const string Sprint = "sprint";

	//"Shift-click"
	[UsedImplicitly] public const string Shift = "shift";

	//"Ctrl-click"
	[UsedImplicitly] public const string Ctrl = "ctrl";

	//"Jump"
	[UsedImplicitly] public const string Jump = "jump";

	//"Sit down"
	[UsedImplicitly] public const string SitDown = "sitdown";

	//"Open Inventory"
	[UsedImplicitly] public const string InventoryDialog = "inventorydialog";

	//"Open character Inventory"
	[UsedImplicitly] public const string CharacterDialog = "characterdialog";

	//"Drop one item"
	[UsedImplicitly] public const string DropItem = "dropitem";

	//"Drop all items"
	[UsedImplicitly] public const string DropItems = "dropitems";

	//"Select Tool Mode"
	[UsedImplicitly] public const string ToolModeSelect = "toolmodeselect";

	//"Show/Hide distance to spawn"
	[UsedImplicitly] public const string CoordinatesHud = "coordinateshud";

	//"Show/Hide block and entity info overlay"
	[UsedImplicitly] public const string BlockInfoHud = "blockinfohud";

	//"Show/Hide block and entity interaction info overlay"
	[UsedImplicitly] public const string BlockInteractionHelp = "blockinteractionhelp";

	//"Show/Hide escape menu dialog"
	[UsedImplicitly] public const string EscapeMenuDialog = "escapemenudialog";

	//"Hide/Show HUD"
	[UsedImplicitly] public const string ToggleHud = "togglehud";

	//"First-, Third-person or Overhead camera"
	[UsedImplicitly] public const string CycleCamera = "cyclecamera";

	//"3rd Person Camera: Zoom out"
	[UsedImplicitly] public const string ZoomOut = "zoomout";

	//"3rd Person Camera: Zoom in"
	[UsedImplicitly] public const string ZoomIn = "zoomin";

	//"Lock/Unlock Mouse Cursor"
	[UsedImplicitly] public const string ToggleMouseControl = "togglemousecontrol";

	//"Chat: Begin Typing"
	[UsedImplicitly] public const string BeginChat = "beginchat";

	//"Chat: Begin Typing a client command"
	[UsedImplicitly] public const string BeginClientCommand = "beginclientcommand";

	//"Chat: Begin Typing a server command"
	[UsedImplicitly] public const string BeginServerCommand = "beginservercommand";

	//"Chat: Show/Hide chat dialog"
	[UsedImplicitly] public const string ChatDialog = "chatdialog";

	//"Open Macro Editor"
	[UsedImplicitly] public const string MacroEditor = "macroeditor";

	//"Toggle Fullscreen mode"
	[UsedImplicitly] public const string ToggleFullscreen = "togglefullscreen";

	//"Take screenshot"
	[UsedImplicitly] public const string Screenshot = "screenshot";

	//"Take mega screenshot"
	[UsedImplicitly] public const string MegaScreenshot = "megascreenshot";

	//"Flip left/right hand contents"
	[UsedImplicitly] public const string FlipHandSlots = "fliphandslots";

	//"Select Hotbar Slot 1"
	[UsedImplicitly] public const string HotbarSlot1 = "hotbarslot1";

	//"Select Hotbar Slot 2"
	[UsedImplicitly] public const string HotbarSlot2 = "hotbarslot2";

	//"Select Hotbar Slot 3"
	[UsedImplicitly] public const string HotbarSlot3 = "hotbarslot3";

	//"Select Hotbar Slot 4"
	[UsedImplicitly] public const string HotbarSlot4 = "hotbarslot4";

	//"Select Hotbar Slot 5"
	[UsedImplicitly] public const string HotbarSlot5 = "hotbarslot5";

	//"Select Hotbar Slot 6"
	[UsedImplicitly] public const string HotbarSlot6 = "hotbarslot6";

	//"Select Hotbar Slot 7"
	[UsedImplicitly] public const string HotbarSlot7 = "hotbarslot7";

	//"Select Hotbar Slot 8"
	[UsedImplicitly] public const string HotbarSlot8 = "hotbarslot8";

	//"Select Hotbar Slot 9"
	[UsedImplicitly] public const string HotbarSlot9 = "hotbarslot9";

	//"Select Hotbar Slot 10"
	[UsedImplicitly] public const string HotbarSlot10 = "hotbarslot10";

	//"Select Backpack Slot 1"
	[UsedImplicitly] public const string BackPackSlot1 = "hotbarslot11";

	//"Select Backpack Slot 2"
	[UsedImplicitly] public const string BackPackSlot2 = "hotbarslot12";

	//"Select Backpack Slot 3"
	[UsedImplicitly] public const string BackPackSlot3 = "hotbarslot13";

	//"Select Backpack Slot 4"
	[UsedImplicitly] public const string BackPackSlot4 = "hotbarslot14";

	//"-1 Fly/Move Speed"
	[UsedImplicitly] public const string DecreaseSpeed = "decspeed";

	//"+1 Fly/Move Speed"
	[UsedImplicitly] public const string IncreaseSpeed = "incspeed";

	//"-0.1 Fly/Move Speed"
	[UsedImplicitly] public const string DecreaseSpeedFraction = "decspeedfrac";

	//"+0.1 Fly/Move Speed"
	[UsedImplicitly] public const string IncreaseSpeedFraction = "incspeedfrac";

	//"Cycle through 3 fly modes"
	[UsedImplicitly] public const string CycleFlyModes = "cycleflymodes";

	//"Fly Mode On/Off"
	[UsedImplicitly] public const string Fly = "fly";

	//"Show/Hide Meta Blocks"
	[UsedImplicitly] public const string RenderMetaBlocks = "rendermetablocks";

	//"FPS graph"
	[UsedImplicitly] public const string FpsGraph = "fpsgraph";

	//"Debug screen + FPS graph"
	[UsedImplicitly] public const string DebugScreenGraph = "debugscreenandgraph";

	//"Reload world"
	[UsedImplicitly] public const string ReloadWorld = "reloadworld";

	//"Reload shaders"
	[UsedImplicitly] public const string ReloadShaders = "reloadshaders";

	//"Reload textures"
	[UsedImplicitly] public const string ReloadTextures = "reloadtextures";

	//"Compact large object heap"
	[UsedImplicitly] public const string CompactHeap = "compactheap";

	//"Recompose all dialogs"
	[UsedImplicitly] public const string RecomposeAllGuis = "recomposeallguis";

	//"Cycle Dialog Outline Modes"
	[UsedImplicitly] public const string CycleDialogOutlineModes = "cycledialogoutlines";

	//"Toggle Tick Profiler"
	[UsedImplicitly] public const string TickProfiler = "tickprofiler";

	//"Pick block"
	[UsedImplicitly] public const string PickBlock = "pickblock";
}