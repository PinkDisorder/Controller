using System;
using System.Collections.Generic;
using Controller.Lib.Util;
using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;

namespace Controller.Lib;

public sealed class State : IRenderer {
	private readonly Dictionary<int, ButtonInput> _buttonsByCode = new();
	private readonly Dictionary<string, ButtonInput> _buttonsByName = new();

	public readonly AnalogStick LeftStick = new(0, 0);
	public readonly AnalogStick RightStick = new(0, 0);

	public readonly JoystickInfo JoystickInfo = new();

	public double RenderOrder => 1d;
	public int RenderRange => 0;

	private readonly string[] _knownButtons = [
		"A", "B", "X", "Y", "Rb", "Lb", "Rt", "Lt", "R3", "L3", "Guide",
		"Start", "Back", "DPadUp", "DPadDown", "DPadLeft", "DPadRight",
	];

	public State() {
		Register(_knownButtons.Select(s => new ButtonInput(s)).ToArray());
	}

	private void Register(params ButtonInput[] buttons) {
		foreach (ButtonInput button in buttons) {
			_buttonsByName.Add(button.Name, button);
			_buttonsByCode.Add(button.Code, button);
		}
	}

	public ButtonInput Get(string name) {
		return _buttonsByName[name];
	}

	public bool Contains(string name) {
		return _buttonsByName.ContainsKey(name);
	}

	public bool Contains(int code) {
		return _buttonsByCode.ContainsKey(code);
	}

	public void Dispose() { }

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Immediately exit on no joystick
		if (JoystickInfo.Id < 0) return;
		// Immediately exit on disconnected joystick.
		if (!GLFW.JoystickPresent(JoystickInfo.Id)) return;
		// No support for axis-less joysticks.
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(JoystickInfo.Id);
		if (axes.IsEmpty) return;

		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(JoystickInfo.Id);

		for (int buttonCode = 0; buttonCode < buttons.Length; buttonCode++) {
			// immediately exit if we're not tracking this button.
			if (!_buttonsByCode.TryGetValue(buttonCode, out ButtonInput button)) return;
			switch (buttons[buttonCode]) {
				case JoystickInputAction.Press:
					button.OnPress(deltaTime);
					break;
				case JoystickInputAction.Release when button.IsActive:
					button.OnRelease();
					break;
			}
		}

		LeftStick.Update(JoystickInfo.LeftAxisX, JoystickInfo.LeftAxisY, axes);
		RightStick.Update(JoystickInfo.RightAxisX, JoystickInfo.RightAxisY, axes, invertY: true);
	}
}