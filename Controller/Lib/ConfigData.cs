using System.Collections.Generic;

namespace Controller.Lib;

public class ConfigData {

	public readonly Dictionary<string, float> Tuning = new() {
		{ "StickDeadzone", 0.15f },
		{ "TriggerDeadzone", 0.3f },
		{ "SensitivityPitch", 0.1f },
		{ "SensitivityYaw", 0.05f }
	};

// keybinds
	public readonly Dictionary<string, string> Keybinds = new() {
		{ "Jump", "A" },
		{ "SwitchHands", "B" },
		{ "SelectTool", "X" },
		{ "Inventory", "Y" },
		{ "DropItem", "DPadUp" },
		{ "CharacterPanel", "DPadDown" },
		{ "HotbarLeft", "DPadLeft" },
		{ "HotbarRight", "DPadRight" },
		{ "LeftClick", "RightTrigger" },
		{ "RightClick", "RightBumper" },
		{ "Sprint", "LeftThumb" },
		{ "Sneak", "RightThumb" },
		{ "Menu", "Start" },
		{ "Chat", "Guide" },
		{ "Map", "Back" }
	};

}
