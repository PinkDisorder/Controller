#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Controller.Enums;

namespace Controller.Lib;

public class InputMonitor : IRenderer {
	// TODO: make these configurable
	public const float Deadzone = 0.15f;
	private const float NoiseThreshold = 0.02f;

	private readonly Dictionary<int, bool> _buttonStates;
	private readonly Dictionary<int, float> _axisStates;
	private readonly int? _primaryJoystick;

	public event Action<int, int>? OnButtonDown;
	public event Action<int, int>? OnButtonUp;
	public event Action<int, Stick, float, float>? OnStickUpdate;
	public event Action<int, Trigger, float>? OnTriggerUpdate;

	// IRenderer properties
	public double RenderOrder => 1.0;
	public int RenderRange => 0;

	public InputMonitor(ILogger logger) {
		_primaryJoystick = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);

		if (_primaryJoystick == -1) _primaryJoystick = null;

		_buttonStates = Enumerable.Range(0, 32).ToDictionary(i => i, _ => false);
		_axisStates = Enumerable.Range(0, 32).ToDictionary(i => i, _ => 0f);
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) => Update();

	private void Update() {
		if (!_primaryJoystick.HasValue) return;
		int jid = _primaryJoystick.Value;

		if (!GLFW.JoystickPresent(jid)) return;

		// Buttons
		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(jid);
		for (int b = 0; b < buttons.Length; b++) {
			HandleButtonChange(jid, b, buttons[b] == JoystickInputAction.Press);
		}

		// Axes
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(jid);
		if (axes.IsEmpty) return;

		HandleStick(jid, Stick.Left, 0, 1, axes);
		HandleStick(jid, Stick.Right, 2, 5, axes);

		HandleTrigger(jid, Trigger.Lt, 3, axes);
		HandleTrigger(jid, Trigger.Rt, 4, axes);
	}

	private void HandleButtonChange(int jid, int button, bool pressed) {
		bool oldState = _buttonStates[button];
		if (pressed && !oldState) {
			_buttonStates[button] = true;
			OnButtonDown?.Invoke(jid, button);
		}
		else if (!pressed && oldState) {
			_buttonStates[button] = false;
			OnButtonUp?.Invoke(jid, button);
		}
	}

	private void HandleStick(int jid, Stick stick, int xAxis, int yAxis, ReadOnlySpan<float> axes) {
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

	private void HandleTrigger(int jid, Trigger trigger, int axis, ReadOnlySpan<float> axes) {
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