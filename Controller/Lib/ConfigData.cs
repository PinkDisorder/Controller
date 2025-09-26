using System;
using Controller.Enums;

namespace Controller.Lib;

public class ConfigData {

	public float Deadzone = 0.15f;

	// keybinds
	public string Jump = "A";
	public string SwitchHands = "B";
	public string SelectTool = "X";
	public string Inventory = "Y";
	public string DropItem = "DPadUp";
	public string CharacterPanel = "DPadDown";
	public string LeftClick = "RightTrigger";
	public string RightClick = "RightBumper";
	public string Sprint = "LeftThumb";
	public string Sneak = "RightThumb";
	public string Menu = "Start";
	public string Chat = "Guide";
	public string Map = "Back";

	public float SensitivityYaw = 0.05f;
	public float SensitivityPitch = 0.1f;
	
	public float TriggerDeadzone = 0.3f;
	
}
