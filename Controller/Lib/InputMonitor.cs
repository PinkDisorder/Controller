#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Enums;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class InputMonitor : IRenderer {
	// TODO: make these configurable

	public const float Deadzone = 0.15f;
	private const float NoiseThreshold = 0.02f;

	private readonly Dictionary<int, bool> _buttonStates;
	private readonly Dictionary<int, float> _axisStates;

	public readonly int? PrimaryJoystick;

	public event Action<int, int>? OnPress;
	public event Action<int, int>? OnRelease;

	public event Action<int, Stick, float, float>? OnStickUpdate;

	// There's not much sense in maintaining analog triggers.
	// Keep the code tho cuz who knows.
	// public event Action<int, Trigger, float>? OnTriggerUpdate;

	private readonly ILogger _logger;
	private readonly State _state;

	// IRenderer properties
	public double RenderOrder => 1.0;
	public int RenderRange => 0;

	public InputMonitor(ILogger logger, State state) {
		_logger = logger;
		_state = state;
		// Scan for a joystick
		PrimaryJoystick = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);

		if (PrimaryJoystick == -1) {
			PrimaryJoystick = null;
		}

		_buttonStates = Enumerable.Range(0, 32).ToDictionary(i => i, _ => false);
		_axisStates = Enumerable.Range(0, 32).ToDictionary(i => i, _ => 0f);
	}

	// Poll for inputs on every frame.
	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Immediately exit on no joystick
		if (!PrimaryJoystick.HasValue) return;
		int jid = PrimaryJoystick.Value;
		// Immediately exit on disconnected joystick.
		if (!GLFW.JoystickPresent(jid)) return;
		// While I'd like to support potentially axis-less controllers
		// where movement would be performed with the dpad, I simply
		// don't think there's enough buttons to handle this.
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(jid);
		if (axes.IsEmpty) return;

		// _logger.Debug($"[controller] found controller with name: {GLFW.GetJoystickName(jid)}");
		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(jid);

		for (int b = 0; b < buttons.Length; b++) {
			UpdateButtonState(jid, b, buttons[b] == JoystickInputAction.Press, deltaTime);
		}

		// TODO: Decouple from DualSense
		UpdateStickState(jid, Stick.Left, 0, 1, axes);
		UpdateStickState(jid, Stick.Right, 2, 5, axes);

		// TODO: Decouple from DualSense
		// There's not much sense in maintaining analog triggers.
		// Keep the code tho cuz who knows.
		// UpdateTriggerState(jid, Trigger.Lt, 3, axes);
		// UpdateTriggerState(jid, Trigger.Rt, 4, axes);
	}

	private void UpdateButtonState(int jid, int button, bool pressed, float deltaTime) {
		bool wasPressed = _buttonStates[button];

		switch (pressed) {
			case true:
				_buttonStates[button] = true;
				_state.Get(button).OnPress(deltaTime);
				OnPress?.Invoke(jid, button);
				break;
			case false when wasPressed:
				_buttonStates[button] = false;
				_state.Get(button).OnRelease();
				OnRelease?.Invoke(jid, button);
				break;
		}
	}

	private void UpdateStickState(int jid, Stick stick, int xAxis, int yAxis, ReadOnlySpan<float> axes) {
		if (axes.Length <= Math.Max(xAxis, yAxis)) return;

		float x = Math.Abs(axes[xAxis]) < Deadzone ? 0f : axes[xAxis];
		float y = Math.Abs(axes[yAxis]) < Deadzone ? 0f : axes[yAxis];

		float oldX = _axisStates[xAxis];
		float oldY = _axisStates[yAxis];

		bool xDidMove = Math.Abs(oldX - x) > NoiseThreshold;
		bool yDidMove = Math.Abs(oldY - y) > NoiseThreshold;

		if (xDidMove || yDidMove) {
			_axisStates[xAxis] = x;
			_axisStates[yAxis] = y;
			OnStickUpdate?.Invoke(jid, stick, x, y);
		}
	}

	// There's not much sense in maintaining analog triggers.
	// Keep the code tho cuz who knows.
	// private void UpdateTriggerState(int jid, Trigger trigger, int axis, ReadOnlySpan<float> axes) {
	// 	if (axes.Length <= axis) return;
	//
	// 	float value = axes[axis];
	//
	// 	value = Math.Abs(value) < Deadzone ? 0f : value;
	//
	// 	if (Math.Abs(_axisStates[axis] - value) > NoiseThreshold) {
	// 		_axisStates[axis] = value;
	// 		OnTriggerUpdate?.Invoke(jid, trigger, value);
	// 	}
	// }

	public void Dispose() { }
}