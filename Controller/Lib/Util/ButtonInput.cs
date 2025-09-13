using System;
using Controller.Enums;

namespace Controller.Lib.Util;

public class ButtonInput(string name) {
	
	public readonly int Code = Core.Config.ControllerType switch {
		"PS5" => (int)Enum.Parse<DualSense>(name, true),
		"XBOX" => (int)Enum.Parse<Xbox>(name, true),
		"SWITCH" => (int)Enum.Parse<Switch>(name, true),
		_ => -1
	};

	public readonly string Name = name;

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
}