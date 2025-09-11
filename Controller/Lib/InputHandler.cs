using Controller.Enums;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class InputHandler(ICoreClientAPI api, State state, int? jid) {
	public void HandleLeftStick(float x, float y) => state.LeftStick.Update(x, y);


	private void TriggerHotKey(HotkeyCode internalKeyCode) {
		HotKey key = api.Input.GetHotKeyByCode(internalKeyCode.ToString());
		key.Handler(key.CurrentMapping);
	}

	// Call me every tick
	public void ApplyInputs() {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		if (jid == null) return;

		player.Controls.Jump = state.Get("A").IsActive;
		player.Controls.Sprint = state.Get("L3").IsActive;
		player.Controls.Sneak = state.Get("R3").IsActive;

		if (state.Get("Guide").IsPressed) {
			TriggerHotKey(HotkeyCode.BeginChat);
		}

		if (state.Get("DPadUp").IsPressed) {
			TriggerHotKey(HotkeyCode.ZoomIn);
		}

		if (state.Get("DPadDown").IsPressed) {
			TriggerHotKey(HotkeyCode.ZoomOut);
		}

		if (state.Get("DPadUp").IsHeld) {
			TriggerHotKey(HotkeyCode.CycleCamera);
		}
		

		player.Controls.Forward = state.LeftStick.Y < -InputMonitor.Deadzone;
		player.Controls.Right = state.LeftStick.X > InputMonitor.Deadzone;
		player.Controls.Backward = state.LeftStick.Y > InputMonitor.Deadzone;
		player.Controls.Left = state.LeftStick.X < -InputMonitor.Deadzone;
	}
}