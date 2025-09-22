using System;
using Controller.Enums;

namespace Controller.Lib.Util;

public class Button(string name) {
	public readonly int Code = Core.Config.ControllerType switch {
		"PS5" => (int)Enum.Parse<DualSense>(name, true),
		"XBOX" => (int)Enum.Parse<Xbox>(name, true),
		"SWITCH" => (int)Enum.Parse<Switch>(name, true),
		_ => -1
	};

	public readonly string Name = name;

	private const float HeldThreshold = 0.5f; // for repeat
	private const float RepeatInterval = 0.2f; // for repeat
	private const float LongPressThreshold = 1.0f; // for long press (example)

	private float _deltaTime;
	private float _nextRepeat;
	private bool _longPressTriggered;

	public bool IsPressed { get; private set; }
	public bool IsHeldRepeat { get; private set; }
	public bool IsLongPressed { get; private set; }
	public bool IsActive => _deltaTime > 0;
	public bool IsReleased { get; private set; }

	public void RegisterPress(float deltaTime) {
		// Reset per-frame transient flags
		IsPressed = false;
		IsHeldRepeat = false;
		IsLongPressed = false;
		IsReleased = false;

		if (_deltaTime == 0) {
			// fresh press
			_nextRepeat = HeldThreshold;
			_longPressTriggered = false;
			IsPressed = true;
		}

		_deltaTime += deltaTime;

		// repeat logic
		if (_deltaTime >= _nextRepeat) {
			IsHeldRepeat = true;
			_nextRepeat += RepeatInterval;
		}

		// long press logic
		if (!(_deltaTime >= LongPressThreshold) || _longPressTriggered) return;
		IsLongPressed = true;
		_longPressTriggered = true;
	}

	public void RegisterRelease() {
		_deltaTime = 0;
		_nextRepeat = 0;
		_longPressTriggered = false;

		IsPressed = false;
		IsHeldRepeat = false;
		IsLongPressed = false;
		IsReleased = true;
	}
}