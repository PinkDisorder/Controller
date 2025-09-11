using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Controller.Lib;

public class InputHandler(ICoreClientAPI api, int? jid) {
	private readonly Vec2f _leftStick = new(0, 0);

	private readonly State _state = new();

	public void HandlePress(int _, int button) => _state.Get(button)?.OnPress();
	public void HandleRelease(int _, int button) => _state.Get(button)?.OnRelease();
	public void HandleLeftStick(float x, float y) => (_leftStick.X, _leftStick.Y) = (x, y);


	private void TriggerHotKey(string internalKeyCode) {
		HotKey key = api.Input.GetHotKeyByCode(internalKeyCode);
		key.Handler(key.CurrentMapping);
	}

	// Call me every tick
	public void ApplyInputs() {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		if (jid == null) return;

		player.Controls.Jump = _state.Get("A").IsPressed || _state.Get("A").IsHeld;
		player.Controls.Sprint = _state.Get("L3").IsHeld;
		player.Controls.Sneak = _state.Get("R3").IsPressed;

		player.Controls.Forward = _leftStick.Y < -InputMonitor.Deadzone;
		player.Controls.Right = _leftStick.X > InputMonitor.Deadzone;
		player.Controls.Backward = _leftStick.Y > InputMonitor.Deadzone;
		player.Controls.Left = _leftStick.X < -InputMonitor.Deadzone;
	}
}