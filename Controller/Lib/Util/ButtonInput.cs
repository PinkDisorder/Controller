using System;
using Controller.Enums;

namespace Controller.Lib.Util;

public class ButtonInput {
	public readonly int Code;
	public readonly string Name;
	private int HeldCounter { get; set; }

	public bool IsPressed => HeldCounter > 0;
	public bool IsHeld => HeldCounter > _heldThreshold;

	public void OnPress() {
		// No point in exceeding this.
		if (HeldCounter > _heldThreshold) return;
		HeldCounter++;
	}


	private readonly int _heldThreshold;

	public ButtonInput(string name, int heldThreshold = 30) {
		_heldThreshold = heldThreshold;

		Name = name;

		if (name.Contains("DualSense")) {
			Enum.TryParse(name, out DualSense ds);
			Code = (int)ds;
			return;
		}

		if (name.Contains("Switch")) {
			Enum.TryParse(name, out Switch sw);
			Code = (int)sw;
			return;
		}

		Enum.TryParse(name, out Xbox xb);
		Code = (int)xb;
	}


	public void OnRelease() {
		HeldCounter = 0;
	}
}