using System;
using Controller.Enums;

namespace Controller.Lib.Util;

public class ButtonInput {
	public readonly int Code;
	public readonly string Name;

	private const float HeldThreshold = 0.5f;
	
	private const float DeltaThreshold = HeldThreshold + 0.1f;

	private float DeltaTime { get; set; }

	public bool IsPressed => DeltaTime is > 0 and < HeldThreshold;

	public bool IsHeld => DeltaTime > HeldThreshold;

	public bool IsActive => DeltaTime > 0;
	
	public void OnPress(float deltaTime) {
		// No point in exceeding this.
		if (DeltaTime >= DeltaThreshold) return;
		DeltaTime += deltaTime;
	}

	public void OnRelease() => DeltaTime = 0;

	public ButtonInput(string name) {
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
}