using Vintagestory.API.Client;

namespace Controller.Lib;

public class Controls {
	private readonly ICoreClientAPI Api;
	private readonly State State;
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
		if (Api.World.Player?.Entity == null) return;
		HotKey key = Api.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	public Controls(ICoreClientAPI api, State state) {
		Api   = api;
		State = state;

		var c = Core.Config;
		State.GetButton(c.Inventory).OnPress      += ToggleInventory;
		State.GetButton(c.SwitchHands).OnPress    += FlipHandSlots;
		State.GetButton(c.SelectTool).OnPress     += SelectTool;
		State.GetButton(c.CharacterPanel).OnPress += CharacterDialog;
		State.GetButton(c.DropItem).OnPress       += DropItem;
		State.GetButton(c.DropItem).OnLongPress   += DropItems;
		State.GetButton(c.Menu).OnPress           += EscapeMenuDialog;
		State.GetButton(c.Chat).OnPress           += ChatDialog;
		State.GetButton(c.LeftClick).OnPress      += PrimaryMouse;
		State.GetButton(c.RightClick).OnPress     += SecondaryMouse;
	}

	/// <summary>
	/// Reserved for checking boolean inputs.
	/// </summary>
	public void ApplyInputs() {
		var player = Api.World.Player?.Entity;
		if (player == null) return;

		var c = Core.Config;
		player.Controls.Forward  = State.LeftStick.Y < -c.Deadzone;
		player.Controls.Backward = State.LeftStick.Y > c.Deadzone;
		player.Controls.Left     = State.LeftStick.X < -c.Deadzone;
		player.Controls.Right    = State.LeftStick.X > c.Deadzone;
		player.Controls.Jump     = State.GetButton(c.Jump).IsActive;
		player.Controls.Sprint   = State.GetButton(c.Sprint).IsActive;
		player.Controls.Sneak    = State.GetButton(c.Sneak).IsActive;
	}

	public void Dispose() {
		var c = Core.Config;
		State.GetButton(c.Inventory).OnPress      -= ToggleInventory;
		State.GetButton(c.SwitchHands).OnPress    -= FlipHandSlots;
		State.GetButton(c.SelectTool).OnPress     -= SelectTool;
		State.GetButton(c.CharacterPanel).OnPress -= CharacterDialog;
		State.GetButton(c.DropItem).OnPress       -= DropItem;
		State.GetButton(c.DropItem).OnLongPress   -= DropItems;
		State.GetButton(c.Menu).OnPress           -= EscapeMenuDialog;
		State.GetButton(c.Chat).OnPress           -= ChatDialog;
		State.GetButton(c.LeftClick).OnPress      -= PrimaryMouse;
		State.GetButton(c.RightClick).OnPress     -= SecondaryMouse;

		var player = Api.World.Player?.Entity;
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
