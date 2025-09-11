using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Controller.Lib;

public class InputHandler(ICoreClientAPI api, State state, int? jid) {
	public void HandlePress(int _, int button) => state.Get(button)?.OnPress();
	public void HandleRelease(int _, int button) => state.Get(button)?.OnRelease();
	public void HandleLeftStick(float x, float y) => state.LeftStick.Update(x, y);


	private void TriggerHotKey(string internalKeyCode) {
		HotKey key = api.Input.GetHotKeyByCode(internalKeyCode);
		key.Handler(key.CurrentMapping);
	}

	// Call me every tick
	public void ApplyInputs() {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		if (jid == null) return;

		player.Controls.Jump = state.Get("A").IsPressed || state.Get("A").IsHeld;
		player.Controls.Sprint = state.Get("L3").IsHeld;
		player.Controls.Sneak = state.Get("R3").IsPressed;

		player.Controls.Forward = state.LeftStick.Y < -InputMonitor.Deadzone;
		player.Controls.Right = state.LeftStick.X > InputMonitor.Deadzone;
		player.Controls.Backward = state.LeftStick.Y > InputMonitor.Deadzone;
		player.Controls.Left = state.LeftStick.X < -InputMonitor.Deadzone;
	}
}