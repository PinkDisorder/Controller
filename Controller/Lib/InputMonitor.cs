#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Lib.Util;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class InputMonitor : IRenderer {
	// TODO: make these configurable


	private readonly Dictionary<int, float> _axisStates;
	private readonly ILogger _logger;
	private readonly InputState _inputState;

	public double RenderOrder => 1d;
	public int RenderRange => 0;

	public readonly int JoyStickId = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);


	public InputMonitor(ILogger logger, InputState inputState) {
		_logger = logger;
		_inputState = inputState;

		_axisStates = Enumerable.Range(0, 32).ToDictionary(i => i, _ => 0f);
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Immediately exit on no joystick
		if (JoyStickId < 0) return;
		// Immediately exit on disconnected joystick.
		if (!GLFW.JoystickPresent(JoyStickId)) return;
		// No support for axis-less joysticks.
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(JoyStickId);
		if (axes.IsEmpty) return;

		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(JoyStickId);

		for (int buttonCode = 0; buttonCode < buttons.Length; buttonCode++) {
			UpdateButtonState(JoyStickId, buttonCode, buttons[buttonCode], deltaTime);
		}

		// TODO: Decouple from DualSense
		_inputState.LeftStick.Update(0, 1, axes);
		_inputState.RightStick.Update(2, 5, axes, invertY: true);
	}

	private void UpdateButtonState(int _, int buttonCode, JoystickInputAction action, float deltaTime) {
		// immediately exit if we're not tracking this button.
		bool exists = _inputState.Contains(buttonCode);
		if (!exists) return;
		ButtonInput button = _inputState.Get(buttonCode);
		switch (action) {
			case JoystickInputAction.Press:
				button.OnPress(deltaTime);
				break;
			case JoystickInputAction.Release when button.IsActive:
				button.OnRelease();
				break;
		}
	}


	public void Dispose() { }
}