using Controller.Lib.Util;
using Controller.Enums;
using JetBrains.Annotations;
using Vintagestory.API.Client;

namespace Controller.Lib;

using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class State : IRenderer {

	private readonly Dictionary<GamepadButton, Button> _buttons = new();

	private readonly Dictionary<GamepadAxis, Button> _triggers = new() {
		{ GamepadAxis.LeftTrigger, new Button() }, { GamepadAxis.RightTrigger, new Button() }
	};

	public AnalogStick LeftStick { get; }
	public AnalogStick RightStick { get; }

	private readonly JoystickInfo j = new();

	public State(ICoreClientAPI api) {
		foreach (GamepadButton btn in Enum.GetValues(typeof(GamepadButton))) {
			_buttons[btn] = new Button();
		}

		LeftStick  = new AnalogStick((int)GamepadAxis.LeftX, (int)GamepadAxis.LeftY, 0, 0);
		RightStick = new AnalogStick((int)GamepadAxis.RightX, (int)GamepadAxis.RightY, 0, 0);
	}

	public unsafe void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		if (j.Id < 0) return;
		if (!GLFW.JoystickPresent(j.Id)) return;

		if (!GLFW.GetGamepadState(j.Id, out var gamepadState)) return;

		foreach ((GamepadButton btn, Button button) in _buttons) {
			byte state = gamepadState.Buttons[(int)btn];

			switch ((InputAction)state) {
				case InputAction.Press:
					button.RegisterPress(deltaTime);
					break;
				case InputAction.Release when button.IsActive:
					button.RegisterRelease();
					break;
				case InputAction.Repeat:
					button.RegisterPress(deltaTime);
					break;
			}
		}

		// The length of the axes will always be 6 as per glfw documentation.
		var axes = new ReadOnlySpan<float>(gamepadState.Axes, 6);

		LeftStick.Update(axes);
		RightStick.Update(axes, invertY: true);

		// Pretend the triggers are buttons
		float leftTrigger  = axes[(int)GamepadAxis.LeftTrigger];
		float rightTrigger = axes[(int)GamepadAxis.RightTrigger];

		UpdateTriggerButton(_triggers[GamepadAxis.LeftTrigger], leftTrigger, deltaTime);
		UpdateTriggerButton(_triggers[GamepadAxis.RightTrigger], rightTrigger, deltaTime);
	}

	private void UpdateTriggerButton(Button button, float value, float deltaTime, float threshold = 0.3f) {
		if (value > threshold) {
			button.RegisterPress(deltaTime);
		}
		else {
			if (button.IsActive) {
				button.RegisterRelease();
			}
		}
	}

	public Button GetButton(GamepadButton btn) => _buttons[btn];

	[CanBeNull]
	public Button GetButton(string name) {
		if (name == "LeftTrigger") {
			return _triggers[GamepadAxis.LeftTrigger];
		}

		if (name == "RightTrigger") {
			return _triggers[GamepadAxis.RightTrigger];
		}

		if (Enum.TryParse<GamepadButton>(name, ignoreCase: true, out var btn)) {
			if (_buttons.TryGetValue(btn, out var button)) return button;
		}

		var names = new List<string>(Enum.GetNames(typeof(GamepadButton)));

		Core.Logger.Error(
			$"Received user defined keybind {name} but there's no such known key name. "
			+ $"Possible key names: {string.Join(", ", Enum.GetNames<GamepadButton>())}."
		);

		return null;
	}

	public void Dispose() { }

	public double RenderOrder => 1d;
	public int RenderRange => 0;

}
