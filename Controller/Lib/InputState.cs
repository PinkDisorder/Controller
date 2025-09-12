using System.Collections.Generic;
using Controller.Lib.Util;
using Vintagestory.API.Util;

namespace Controller.Lib;

public class InputState {
	private readonly Dictionary<int, ButtonInput> _buttonsByCode = new();
	private readonly Dictionary<string, ButtonInput> _buttonsByName = new();

	public readonly AnalogStick LeftStick = new(0, 0);
	public readonly AnalogStick RightStick = new(0, 0);


	private void Register(params ButtonInput[] buttons) {
		foreach (ButtonInput button in buttons) {
			_buttonsByName.Add(button.Name, button);
			_buttonsByCode.Add(button.Code, button);
		}
	}

	public ButtonInput Get(int code) {
		return _buttonsByCode[code];
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

	public InputState() {
		Register(
			new ButtonInput("A"),
			new ButtonInput("B"),
			new ButtonInput("X"),
			new ButtonInput("Y"),
			new ButtonInput("Rb"),
			new ButtonInput("Lb"),
			new ButtonInput("Rt"),
			new ButtonInput("Lt"),
			new ButtonInput("R3"),
			new ButtonInput("L3"),
			new ButtonInput("Start"),
			new ButtonInput("Back"),
			new ButtonInput("DPadUp"),
			new ButtonInput("DPadRight"),
			new ButtonInput("DPadDown"),
			new ButtonInput("DPadLeft"),
			new ButtonInput("Guide")
		);
	}
}