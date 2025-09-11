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

	public event Action<int, int>? OnButtonDown;
	public event Action<int, int>? OnButtonUp;

	public event Action<int, Stick, float, float>? OnStickUpdate;
	public event Action<int, Trigger, float>? OnTriggerUpdate;

	private readonly ILogger _logger;

	// IRenderer properties
	public double RenderOrder => 1.0;
	public int RenderRange => 0;

	public InputMonitor(ILogger logger) {
		_logger = logger;
		PrimaryJoystick = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);

		if (PrimaryJoystick == -1) PrimaryJoystick = null;

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
		string name = GLFW.GetJoystickName(jid);
		_logger.Chat($"[controller] found controller with name: {name}");
		// Buttons
		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(jid);
		for (int b = 0; b < buttons.Length; b++) {
			// TODO: Correct the pressed thing which is actually a Press/Released enum.
			UpdateButtonState(jid, b, buttons[b] == JoystickInputAction.Press);
		}

		// Axes
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(jid);
		if (axes.IsEmpty) return;

		UpdateStickState(jid, Stick.Left, 0, 1, axes);
		UpdateStickState(jid, Stick.Right, 2, 5, axes);

		UpdateTriggerState(jid, Trigger.Lt, 3, axes);
		UpdateTriggerState(jid, Trigger.Rt, 4, axes);
	}

	private void UpdateButtonState(int jid, int button, bool pressed) {
		bool oldState = _buttonStates[button];

		if (pressed && !oldState) {
			_buttonStates[button] = true;
			OnButtonDown?.Invoke(jid, button);
		} else if (!pressed && oldState) {
			_buttonStates[button] = false;
			OnButtonUp?.Invoke(jid, button);
		}
	}

	private void UpdateStickState(int jid, Stick stick, int xAxis, int yAxis, ReadOnlySpan<float> axes) {
		if (axes.Length <= Math.Max(xAxis, yAxis)) return;


		float x = axes[xAxis];
		float y = axes[yAxis];

		x = Math.Abs(x) < Deadzone ? 0f : x;
		y = Math.Abs(y) < Deadzone ? 0f : y;

		float oldX = _axisStates[xAxis], oldY = _axisStates[yAxis];

		if (Math.Abs(oldX - x) > NoiseThreshold || Math.Abs(oldY - y) > NoiseThreshold) {
			_axisStates[xAxis] = x;
			_axisStates[yAxis] = y;
			OnStickUpdate?.Invoke(jid, stick, x, y);
		}
	}

	private void UpdateTriggerState(int jid, Trigger trigger, int axis, ReadOnlySpan<float> axes) {
		if (axes.Length <= axis) return;

		float value = axes[axis];

		value = Math.Abs(value) < Deadzone ? 0f : value;

		if (Math.Abs(_axisStates[axis] - value) > NoiseThreshold) {
			_axisStates[axis] = value;
			OnTriggerUpdate?.Invoke(jid, trigger, value);
		}
	}

	public void Dispose() { }
}