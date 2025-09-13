#nullable enable
using System;
using Controller.Lib.Util;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class InputMonitor(ILogger logger, InputState inputState, JoystickInfo joystickInfo) : IRenderer {
	public double RenderOrder => 1d;
	public int RenderRange => 0;

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) {
		// Immediately exit on no joystick
		if (joystickInfo.Id < 0) return;
		// Immediately exit on disconnected joystick.
		if (!GLFW.JoystickPresent(joystickInfo.Id)) return;
		// No support for axis-less joysticks.
		ReadOnlySpan<float> axes = GLFW.GetJoystickAxes(joystickInfo.Id);
		if (axes.IsEmpty) return;

		ReadOnlySpan<JoystickInputAction> buttons = GLFW.GetJoystickButtons(joystickInfo.Id);

		for (int buttonCode = 0; buttonCode < buttons.Length; buttonCode++) {
			// immediately exit if we're not tracking this button.
			if (!inputState.Contains(buttonCode)) return;
			ButtonInput button = inputState.Get(buttonCode);
			switch (buttons[buttonCode]) {
				case JoystickInputAction.Press:
					button.OnPress(deltaTime);
					break;
				case JoystickInputAction.Release when button.IsActive:
					button.OnRelease();
					break;
			}
		}

		inputState.LeftStick.Update(joystickInfo.LeftAxisX, joystickInfo.LeftAxisY, axes);
		inputState.RightStick.Update(joystickInfo.RightAxisX, joystickInfo.RightAxisY, axes, invertY: true);
	}

	public void Dispose() { }
}