using Controller.Enums;
using Controller.Lib.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class Controls(ICoreClientAPI api, State state) {
	private void TriggerHotKey(string hotkeyCode) {
		HotKey key = api.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	// Call me every tick
	public void ApplyInputs() {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		if (state.JoystickInfo.Id < 0) return;

		player.Controls.Jump = state.Get(Core.Config.Jump).IsActive;
		player.Controls.Sprint = state.Get(Core.Config.Sprint).IsActive;
		player.Controls.Sneak = state.Get(Core.Config.Sneak).IsActive;

		Button inv = state.Get(Core.Config.OpenInventory);
		if (inv.IsPressed && !inv.IsHeldRepeat) {
			TriggerHotKey(HotkeyCode.InventoryDialog);
		}

		if (state.Get("Guide").IsPressed)
			TriggerHotKey(HotkeyCode.BeginChat);

		if (state.Get("DPadUp").IsPressed) {
			TriggerHotKey(HotkeyCode.ZoomIn);
		}

		if (state.Get("DPadDown").IsPressed) {
			TriggerHotKey(HotkeyCode.ZoomOut);
		}

		if (state.Get("DPadUp").IsLongPressed) {
			TriggerHotKey(HotkeyCode.CycleCamera);
		}


		player.Controls.Forward = state.LeftStick.Y < -Core.Config.Deadzone;
		player.Controls.Right = state.LeftStick.X > Core.Config.Deadzone;
		player.Controls.Backward = state.LeftStick.Y > Core.Config.Deadzone;
		player.Controls.Left = state.LeftStick.X < -Core.Config.Deadzone;
	}
}