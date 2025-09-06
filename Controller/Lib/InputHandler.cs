using System;
using Vintagestory.API.Client;
using Controller.Enums;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class InputHandler(ICoreClientAPI api) {
	private bool _isJumpHeld;
	private bool _isSprintHeld;
	private bool _isSneakHeld;

	private readonly Vec2f _leftStick = new(0, 0);
	private readonly Vec2f _rightStick = new(0, 0);

	private ICoreClientAPI Capi { get; set; } = api;

	private const float RightStickSensitivity = 0.05f;
	private double _cameraYaw;
	private double _cameraPitch;

	private bool _cameraInitialized;

	// Call me every tick
	public void ApplyInputs() {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;

		player.Controls.Jump = _isJumpHeld;
		player.Controls.Sprint = _isSprintHeld;
		player.Controls.ShiftKey = _isSneakHeld;

		player.Controls.Forward = _leftStick.Y > InputMonitor.Deadzone;
		player.Controls.Backward = _leftStick.Y < -InputMonitor.Deadzone;
		player.Controls.Right = _leftStick.X > InputMonitor.Deadzone;
		player.Controls.Left = _leftStick.X < -InputMonitor.Deadzone;
	}


	public void OnStickUpdateHandler(int jid, Stick stick, float x, float y) {
		// Y axes need to be inverted on both sticks.
		// Uncertain if this is PS5 controller specific or happens for xbox too.
		// TODO: Config stick axis inverting.
		if (stick == Stick.Left) {
			_leftStick.X = x;
			_leftStick.Y = -y;
		} else if (stick == Stick.Right) {
			_rightStick.X = x;
			_rightStick.Y = y;
		}
	}

	public void ApplyRightStickCamera() {
		var clientPlayer = Capi.World.Player;
		var entity = clientPlayer?.Entity;
		if (entity == null) return;

		if (!_cameraInitialized) {
			_cameraYaw = clientPlayer.CameraYaw;
			_cameraPitch = clientPlayer.CameraPitch;
			_cameraInitialized = true;
		}

		float dx = Math.Abs(_rightStick.X) > InputMonitor.Deadzone ? _rightStick.X * RightStickSensitivity : 0f;
		float dy = Math.Abs(_rightStick.Y) > InputMonitor.Deadzone ? _rightStick.Y * RightStickSensitivity : 0f;

		_cameraYaw -= dx;
		_cameraPitch += dy;

		_cameraPitch = Math.Clamp(_cameraPitch, -89f, 89f);

		clientPlayer.CameraYaw = (float)_cameraYaw;
		clientPlayer.CameraPitch = (float)_cameraPitch;

		Capi.Input.MouseYaw = (float)_cameraYaw;
		Capi.Input.MousePitch = (float)_cameraPitch;

		// Apply horizontal rotation to entity for server sync
		entity.ServerPos.Yaw = (float)_cameraYaw;
	}

	public void OnButtonDownHandler(int jid, int button) {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;


		switch ((Button)button) {
			case Button.A:
				_isJumpHeld = true;
				break;

			case Button.L3:
				_isSprintHeld = true;
				break;

			case Button.R3:
				_isSneakHeld = true;
				break;
		}
	}

	public void OnButtonUpHandler(int jid, int button) {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;


		switch ((Button)button) {
			case Button.A:
				_isJumpHeld = false;
				break;

			case Button.B:
				_isSprintHeld = false;
				break;

			case Button.R3:
				_isSneakHeld = false;
				break;
		}
	}
}