using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Controller.Enums;
using Controller.Lib.Util;

namespace Controller.Lib;

public class State : IRenderer {

	// The length of the axes will always be 6 as per glfw documentation.
	private const int AxesLength = 6;

	private readonly static GamepadButton[] ButtonCodes = (GamepadButton[])Enum.GetValues(typeof(GamepadButton));

	private readonly Dictionary<GamepadButton, Button> Buttons = ButtonCodes.ToDictionary(btn => btn, _ => new Button());

	private readonly Dictionary<string, GamepadButton> GamepadButtonMap = ButtonCodes.ToDictionary(
		btn => btn.ToString(),
		btn => btn,
		StringComparer.OrdinalIgnoreCase
	);

	private readonly Dictionary<GamepadAxis, Button> Triggers = new() {
		{ GamepadAxis.LeftTrigger, new Button() },
		{ GamepadAxis.RightTrigger, new Button() }
	};

	private readonly string ButtonNames = string.Join(", ", Enum.GetNames(typeof(GamepadButton)));

	public readonly AnalogStick LeftStick = new((int)GamepadAxis.LeftX, (int)GamepadAxis.LeftY);
	public readonly AnalogStick RightStick = new((int)GamepadAxis.RightX, (int)GamepadAxis.RightY);

	private readonly int _joystickId = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);

	public Button GetButton(string name) {
		string err = $"Received user defined keybind {name}. This key is unknown. Possible keys: {ButtonNames}.";

		return name switch {
			"LeftTrigger"  => Triggers[GamepadAxis.LeftTrigger],
			"RightTrigger" => Triggers[GamepadAxis.RightTrigger],
			_ => GamepadButtonMap.TryGetValue(name, out GamepadButton key)
				? Buttons[key]
				: throw new KeyNotFoundException(err)
		};
	}

	private static string UnknownState(byte state, GamepadButton btn) =>
		$"Unknown GLFW button state?: Unhandled InputAction: {state} for button {btn}";

	public unsafe void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Early exit if we have no joystick or can't read it.
		if (_joystickId < 0) return;
		if (!GLFW.GetGamepadState(_joystickId, out var gamepadState)) return;


		foreach ((GamepadButton btn, Button button) in Buttons) {
			byte state = gamepadState.Buttons[(int)btn];

			switch ((InputAction)state) {
				case InputAction.Press:  button.RegisterPress(deltaTime); break;
				case InputAction.Repeat: button.RegisterPress(deltaTime); break;

				case InputAction.Release when button.IsActive: button.RegisterRelease(); break;

				// Release is the state sent on every read, no point in
				// wasting cycles unless the button is active.
				case InputAction.Release: break;

				default: Core.Logger.Warning(UnknownState(state, btn)); break;
			}
		}

		var axes = new ReadOnlySpan<float>(gamepadState.Axes, AxesLength);

		LeftStick.Update(axes);
		RightStick.Update(axes, invertY: true);

		// Pretend the triggers are buttons
		float leftTrigger  = axes[(int)GamepadAxis.LeftTrigger];
		float rightTrigger = axes[(int)GamepadAxis.RightTrigger];

		Button ltb = Triggers[GamepadAxis.LeftTrigger];
		Button rtb = Triggers[GamepadAxis.RightTrigger];

		float tdz = Core.Config.Tuning["TriggerDeadzone"];

		if (leftTrigger > tdz) {
			ltb.RegisterPress(deltaTime);
		}
		else if (leftTrigger < tdz && ltb.IsActive) {
			ltb.RegisterRelease();
		}

		if (rightTrigger > tdz) {
			rtb.RegisterPress(deltaTime);
		}
		else if (rightTrigger < tdz && rtb.IsActive) {
			rtb.RegisterRelease();
		}
	}

	public void Dispose() { }

	public double RenderOrder => 1d;
	public int RenderRange => 0;

}
