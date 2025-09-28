using System;

namespace Controller.Lib.Util;

public class Button {

	private const float HeldThreshold = 0.5f;      // for repeat
	private const float RepeatInterval = 0.2f;     // for repeat
	private const float LongPressThreshold = 1.0f; // for long press (example)

	private float _deltaTime;
	private float _nextRepeat;
	private bool _longPressTriggered;

	public bool IsPressed { get; private set; }
	public bool IsHeldRepeat { get; private set; }
	public bool IsLongPressed { get; private set; }
	public bool IsActive => _deltaTime > 0;
	public bool IsReleased { get; private set; }

	public event Action? OnPress;
	public event Action? OnHeldRepeat;
	public event Action? OnLongPress;
	public event Action? OnRelease;

	public void RegisterPress(float deltaTime) {
		// Reset per-frame transient flags
		IsPressed     = false;
		IsHeldRepeat  = false;
		IsLongPressed = false;
		IsReleased    = false;

		if (_deltaTime == 0) {
			// fresh press
			_nextRepeat         = HeldThreshold;
			_longPressTriggered = false;
			IsPressed           = true;

			OnPress?.Invoke();
		}

		_deltaTime += deltaTime;

		// repeat logic
		if (_deltaTime >= _nextRepeat) {
			IsHeldRepeat = true;
			OnHeldRepeat?.Invoke();
			_nextRepeat += RepeatInterval;
		}

		// long press logic
		if (!(_deltaTime >= LongPressThreshold) || _longPressTriggered) return;
		IsLongPressed = true;
		OnLongPress?.Invoke();

		_longPressTriggered = true;
	}

	public void RegisterRelease() {
		_deltaTime          = 0;
		_nextRepeat         = 0;
		_longPressTriggered = false;

		IsPressed     = false;
		IsHeldRepeat  = false;
		IsLongPressed = false;
		IsReleased    = true;

		OnRelease?.Invoke();
	}

}
