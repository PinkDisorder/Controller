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

	private static int _joystickId = Enumerable.Range(0, 16).FirstOrDefault(i => GLFW.GetGamepadState(i, out var _), -1);

	public static void JoystickCallback(int jid, ConnectedState state) {
		if (state is ConnectedState.Disconnected) {
			if (jid == _joystickId) {
				_joystickId = -1;
			} else return;
		}

		// stateless device, not a real joystick
		if (!GLFW.GetGamepadState(jid, out var _)) return;

		switch (_joystickId) {
			case < 0:
				_joystickId = jid;
				return;

			case > 0 when GLFW.GetGamepadState(_joystickId, out var _):
				return;

			case > 0:
				_joystickId = jid;
				break;
		}
	}

	private readonly static GamepadButton[] ButtonCodes = Enum.GetValues<GamepadButton>();

	private readonly static string ButtonNames = string.Join(", ", Enum.GetNames(typeof(GamepadButton)));

	private readonly static Dictionary<GamepadButton, Button> Buttons =
		ButtonCodes.ToDictionary(btn => btn, _ => new Button());

	public static AnalogStick LeftStick { get; } = new((int)GamepadAxis.LeftX, (int)GamepadAxis.LeftY);

	public static AnalogStick RightStick { get; } = new((int)GamepadAxis.RightX, (int)GamepadAxis.RightY);

	private readonly static Dictionary<string, GamepadButton> GamepadButtonMap =
		ButtonCodes.ToDictionary(btn => btn.ToString(), btn => btn, StringComparer.OrdinalIgnoreCase);

	private readonly static Dictionary<GamepadAxis, Button> Triggers = new() {
		{ GamepadAxis.LeftTrigger, new Button() },
		{ GamepadAxis.RightTrigger, new Button() }
	};

	public static Button GetButton(string name) {
		string err = $"Received user defined keybind {name}." + $"This key is unknown. Possible keys: {ButtonNames}.";

		return name switch {
			"LeftTrigger"  => Triggers[GamepadAxis.LeftTrigger],
			"RightTrigger" => Triggers[GamepadAxis.RightTrigger],
			var _ => GamepadButtonMap.TryGetValue(name, out GamepadButton key)
				? Buttons[key]
				: throw new KeyNotFoundException(err)
		};
	}

	public unsafe void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		if (_joystickId < 0) return;
		if (!GLFW.GetGamepadState(_joystickId, out var gamepadState)) return;

		foreach ((GamepadButton btn, Button button) in Buttons) {
			byte state = gamepadState.Buttons[(int)btn];

			switch ((InputAction)state) {
				case InputAction.Press:
					button.RegisterPress(deltaTime);
					break;
				case InputAction.Repeat:
					button.RegisterPress(deltaTime);
					break;
				case InputAction.Release when button.IsActive:
					button.RegisterRelease();
					break;
				case InputAction.Release:
				default:
					break;
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
		} else if (leftTrigger < tdz && ltb.IsActive) {
			ltb.RegisterRelease();
		}

		if (rightTrigger > tdz) {
			rtb.RegisterPress(deltaTime);
		} else if (rightTrigger < tdz && rtb.IsActive) {
			rtb.RegisterRelease();
		}
	}

	public void Dispose() { }

	public double RenderOrder => 1d;
	public int RenderRange => 0;

}
