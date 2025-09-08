using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Controller.Enums;


namespace Controller.Lib;

public class InputHandler(ICoreClientAPI api) {
	private readonly Vec2f _leftStick = new(0, 0);


	// Call me every tick
	public void ApplyInputs() {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;

		player.Controls.Jump = State.Buttons.Get(Button.A).IsPressed || State.Buttons.Get(Button.A).IsHeld;
		player.Controls.Sprint = State.Buttons.Get(Button.L3).IsHeld;
		player.Controls.Sneak = State.Buttons.Get(Button.R3).IsPressed;

		player.Controls.Forward = _leftStick.Y < -InputMonitor.Deadzone;
		player.Controls.Right = _leftStick.X > InputMonitor.Deadzone;
		player.Controls.Backward = _leftStick.Y > InputMonitor.Deadzone;
		player.Controls.Left = _leftStick.X < -InputMonitor.Deadzone;
	}


	public void HandleLeftStick(int jid, Stick stick, float x, float y) {
		// TODO: Config stick axis inverting.
		if (stick != Stick.Left) return;
		_leftStick.X = x;
		_leftStick.Y = y;
	}


	private void TriggerHotKey(string internalKeyCode) {
		HotKey key = api.Input.GetHotKeyByCode(internalKeyCode);
		key.Handler(key.CurrentMapping);
	}

	// TODO: Figure out how to handle setting keybinds. A custom UI maybe?
	public void HandleButtonDown(int jid, int button) {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		State.ButtonInput buttonResolved = State.Buttons.Get((Button)button);
		buttonResolved?.OnPress();
	}

	public void HandleButtonUp(int jid, int button) {
		EntityPlayer player = api.World.Player?.Entity;
		if (player == null) return;
		State.ButtonInput buttonResolved = State.Buttons.Get((Button)button);
		buttonResolved?.OnRelease();
	}
}