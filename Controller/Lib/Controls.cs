using Vintagestory.API.Client;

namespace Controller.Lib;

public class Controls {

	private readonly ICoreClientAPI _api;
	private readonly State _state;

	private const int HotbarLength = 10;

	private void ToggleInventory() => TriggerHotKey(HotkeyCode.InventoryDialog);

	private void CharacterDialog() => TriggerHotKey(HotkeyCode.CharacterDialog);

	private void DropItem() => TriggerHotKey(HotkeyCode.DropItem);

	private void DropItems() => TriggerHotKey(HotkeyCode.DropItems);

	private void SelectTool() => TriggerHotKey(HotkeyCode.ToolModeSelect);

	private void EscapeMenuDialog() => TriggerHotKey(HotkeyCode.EscapeMenuDialog);

	private void ChatDialog() => TriggerHotKey(HotkeyCode.ChatDialog);

	private void FlipHandSlots() => TriggerHotKey(HotkeyCode.FlipHandSlots);

	private void PrimaryMouse() => TriggerHotKey(HotkeyCode.PrimaryMouse);

	private void SecondaryMouse() => TriggerHotKey(HotkeyCode.SecondaryMouse);

	private void TriggerHotKey(string hotkeyCode) {
		if (_api.World.Player?.Entity == null) return;
		HotKey key = _api.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	private void HotbarRight() {
		int a = _api.World.Player.InventoryManager.ActiveHotbarSlotNumber;
		_api.World.Player.InventoryManager.ActiveHotbarSlotNumber = (a + 1) % HotbarLength;
	}

	private void HotbarLeft() {
		int a = _api.World.Player.InventoryManager.ActiveHotbarSlotNumber;
		_api.World.Player.InventoryManager.ActiveHotbarSlotNumber = (a - 1 + HotbarLength) % HotbarLength;
	}

	public Controls(ICoreClientAPI api, State state) {
		_api   = api;
		_state = state;

		RegisterListeners();
	}

	private void RegisterListeners() {
		var c = Core.Config.Data;
		_state.GetButton(c.Keybinds["Inventory"]).OnPress      += ToggleInventory;
		_state.GetButton(c.Keybinds["SwitchHands"]).OnPress    += FlipHandSlots;
		_state.GetButton(c.Keybinds["SelectTool"]).OnPress     += SelectTool;
		_state.GetButton(c.Keybinds["CharacterPanel"]).OnPress += CharacterDialog;
		_state.GetButton(c.Keybinds["DropItem"]).OnPress       += DropItem;
		_state.GetButton(c.Keybinds["DropItem"]).OnLongPress   += DropItems;
		_state.GetButton(c.Keybinds["Menu"]).OnPress           += EscapeMenuDialog;
		_state.GetButton(c.Keybinds["Chat"]).OnPress           += ChatDialog;
		_state.GetButton(c.Keybinds["LeftClick"]).OnPress      += PrimaryMouse;
		_state.GetButton(c.Keybinds["RightClick"]).OnPress     += SecondaryMouse;
		_state.GetButton(c.Keybinds["HotbarLeft"]).OnPress     += HotbarLeft;
		_state.GetButton(c.Keybinds["HotbarRight"]).OnPress    += HotbarRight;

		_state.GetButton(c.Keybinds["HotbarLeft"]).OnHeldRepeat  += HotbarLeft;
		_state.GetButton(c.Keybinds["HotbarRight"]).OnHeldRepeat += HotbarRight;
	}

	// Reserved for checking boolean inputs.
	public void ApplyInputs() {
		var player = _api.World.Player?.Entity;
		if (player == null) return;

		var c = Core.Config.Data;
		player.Controls.Forward  = _state.LeftStick.Y < -c.Tuning["StickDeadzone"];
		player.Controls.Backward = _state.LeftStick.Y > c.Tuning["StickDeadzone"];
		player.Controls.Left     = _state.LeftStick.X < -c.Tuning["StickDeadzone"];
		player.Controls.Right    = _state.LeftStick.X > c.Tuning["StickDeadzone"];
		player.Controls.Jump     = _state.GetButton(c.Keybinds["Jump"]).IsActive;
		player.Controls.Sprint   = _state.GetButton(c.Keybinds["Sprint"]).IsActive;
		player.Controls.Sneak    = _state.GetButton(c.Keybinds["Sneak"]).IsActive;
	}

	public void Dispose() {
		var c = Core.Config.Data;
		_state.GetButton(c.Keybinds["Inventory"]).OnPress      -= ToggleInventory;
		_state.GetButton(c.Keybinds["SwitchHands"]).OnPress    -= FlipHandSlots;
		_state.GetButton(c.Keybinds["SelectTool"]).OnPress     -= SelectTool;
		_state.GetButton(c.Keybinds["CharacterPanel"]).OnPress -= CharacterDialog;
		_state.GetButton(c.Keybinds["DropItem"]).OnPress       -= DropItem;
		_state.GetButton(c.Keybinds["DropItem"]).OnLongPress   -= DropItems;
		_state.GetButton(c.Keybinds["Menu"]).OnPress           -= EscapeMenuDialog;
		_state.GetButton(c.Keybinds["Chat"]).OnPress           -= ChatDialog;
		_state.GetButton(c.Keybinds["LeftClick"]).OnPress      -= PrimaryMouse;
		_state.GetButton(c.Keybinds["RightClick"]).OnPress     -= SecondaryMouse;

		_state.GetButton(c.Keybinds["HotbarLeft"]).OnHeldRepeat  -= HotbarLeft;
		_state.GetButton(c.Keybinds["HotbarRight"]).OnHeldRepeat -= HotbarRight;

		var player = _api.World.Player?.Entity;
		if (player == null) return;
		player.Controls.Forward  = false;
		player.Controls.Right    = false;
		player.Controls.Backward = false;
		player.Controls.Left     = false;
		player.Controls.Jump     = false;
		player.Controls.Sprint   = false;
		player.Controls.Sneak    = false;
	}

}
