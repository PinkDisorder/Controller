using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Controller.Enums;
using Controller.Lib.Util;

namespace Controller.Lib;

public class State : IRenderer {

	private readonly static GamepadButton[] ButtonCodes = (GamepadButton[])Enum.GetValues(typeof(GamepadButton));

	private readonly Dictionary<GamepadButton, Button> Buttons = ButtonCodes.ToDictionary(btn => btn, _ => new Button());

	private readonly Dictionary<string, GamepadButton> GamepadButtonMap = ButtonCodes.ToDictionary(
		btn => btn.ToString()
		, btn => btn
		, StringComparer.OrdinalIgnoreCase
	);

	private readonly Dictionary<GamepadAxis, Button> Triggers = new() {
		{ GamepadAxis.LeftTrigger, new Button() }
		, { GamepadAxis.RightTrigger, new Button() }
	};

	private readonly string ButtonNames = string.Join(", ", Enum.GetNames(typeof(GamepadButton)));

	public readonly AnalogStick LeftStick = new((int)GamepadAxis.LeftX, (int)GamepadAxis.LeftY);
	public readonly AnalogStick RightStick = new((int)GamepadAxis.RightX, (int)GamepadAxis.RightY);

	private readonly JoystickInfo _joystickInfo = new();

	public Button GetButton(string name) {
		string err = $"Received user defined keybind {name}. This key is unknown. Possible keys: {ButtonNames}.";

		switch (name) {
			case "LeftTrigger":
				return Triggers[GamepadAxis.LeftTrigger];
			case "RightTrigger":
				return Triggers[GamepadAxis.RightTrigger];
			default:
				return GamepadButtonMap.TryGetValue(name, out GamepadButton key)
					? Buttons[key]
					: throw new KeyNotFoundException(err);
		}
	}

	public unsafe void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Early exit if we have no joystick or can't read it.
		if (_joystickInfo.Id < 0) return;
		if (!GLFW.GetGamepadState(_joystickInfo.Id, out var gamepadState)) return;

		foreach ((GamepadButton btn, Button button) in Buttons) {
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
				case 0:
					break;
				default:
					Core.Logger.Warning($"Unknown GLFW button state?: Unhandled InputAction: {state} for button {btn}");
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

		if (leftTrigger > Core.Config.Data.Tuning["TriggerDeadzone"]) {
			Triggers[GamepadAxis.LeftTrigger].RegisterPress(deltaTime);
		}
		else if (leftTrigger < Core.Config.Data.Tuning["TriggerDeadzone"]
						&& Triggers[GamepadAxis.LeftTrigger].IsActive) {
			Triggers[GamepadAxis.LeftTrigger].RegisterRelease();
		}


		if (rightTrigger > Core.Config.Data.Tuning["TriggerDeadzone"]) {
			Triggers[GamepadAxis.RightTrigger].RegisterPress(deltaTime);
		}
		else if (rightTrigger < Core.Config.Data.Tuning["TriggerDeadzone"]
						&& Triggers[GamepadAxis.RightTrigger].IsActive) {
			Triggers[GamepadAxis.RightTrigger].RegisterRelease();
		}
	}

	public void Dispose() { }

	public double RenderOrder => 1d;
	public int RenderRange => 0;

}
