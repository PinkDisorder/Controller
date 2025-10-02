using System;
using System.Collections.Generic;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public static class Controls {

	private record ControlCallbacks {

		public Action? OnPress { get; init; }
		public Action? OnHeldRepeat { get; init; }
		public Action? OnRelease { get; init; }

	}

	private const int HotbarLength = 10;

	private readonly static int CenterX = Core.Capi.Render.FrameWidth / 2;
	private readonly static int CenterY = Core.Capi.Render.FrameHeight / 2;

	private static int ActiveHotbarSlotNumber {
		get => Core.Capi.World.Player?.InventoryManager.ActiveHotbarSlotNumber ?? 0;
		set {
			if (Core.Capi.World.Player is not null)
				Core.Capi.World.Player.InventoryManager.ActiveHotbarSlotNumber = value;
		}
	}

	private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

	private readonly static EventInfo? MouseDownInfo = Core.Capi.Event.GetType().GetEvent("MouseUp", Flags);
	private readonly static EventInfo? MouseUpInfo = Core.Capi.Event.GetType().GetEvent("MouseDown", Flags);

	private static void TriggerHotKey(string hotkeyCode) {
		if (Core.Capi.World.Player?.Entity == null) return;
		HotKey key = Core.Capi.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	private static void OnMouseInput(int x, int y, EnumMouseButton btn, bool dirIsDown) {
		switch (dirIsDown) {
			case true when MouseDownInfo is null: return;
			case false when MouseUpInfo is null:  return;
		}

		EventInfo? eventInfo = dirIsDown ? MouseDownInfo : MouseUpInfo;
		if (eventInfo is null) return;

		MouseEventDelegate? del =
			(MouseEventDelegate?)Core.Capi.Event.GetType().GetField(eventInfo.Name, Flags)?.GetValue(Core.Capi.Event);

		del?.Invoke(new MouseEvent(x, y, btn));
	}

	private readonly static Dictionary<string, ControlCallbacks> CallbackStore = new() {
		["Inventory"] = new ControlCallbacks() {
			OnPress = ToggleInventory
		},
		["SwitchHands"] = new ControlCallbacks() {
			OnPress = FlipHandSlots
		},
		["SelectTool"] = new ControlCallbacks() {
			OnPress = SelectTool
		},
		["CharacterPanel"] = new ControlCallbacks() {
			OnPress = CharacterDialog
		},
		["DropItem"] = new ControlCallbacks() {
			OnPress = DropItem
		},

		["Menu"] = new ControlCallbacks() {
			OnPress = EscapeMenuDialog
		},
		["Map"] = new ControlCallbacks() {
			OnPress = WorldMapDialog
		},

		["Chat"] = new ControlCallbacks() {
			OnPress = ChatDialog
		},

		["HotbarLeft"] = new ControlCallbacks() {
			OnPress      = HotbarLeft,
			OnHeldRepeat = HotbarLeft
		},
		["HotbarRight"] = new ControlCallbacks() {
			OnPress      = HotbarRight,
			OnHeldRepeat = HotbarRight
		},
		["LeftClick"] = new ControlCallbacks() {
			OnPress      = LeftClickDown,
			OnHeldRepeat = LeftClickDown,
			OnRelease    = LeftClickUp
		},
		["RightClick"] = new ControlCallbacks() {
			OnPress      = RightClickDown,
			OnHeldRepeat = RightClickDown,
			OnRelease    = RightClickUp
		},
	};

	public static void RegisterListeners() {
		foreach ((string keybind, ControlCallbacks callbacks) in CallbackStore) {
			var button = State.GetButton(Core.Config.Keybinds[keybind]);

			if (callbacks.OnPress is not null) {
				button.OnPress += callbacks.OnPress;
			}

			if (callbacks.OnHeldRepeat is not null) {
				button.OnHeldRepeat += callbacks.OnHeldRepeat;
			}

			if (callbacks.OnRelease is not null) {
				button.OnRelease += callbacks.OnRelease;
			}
		}
	}

	public static void UnregisterListeners() {
		foreach ((string keybind, ControlCallbacks callbacks) in CallbackStore) {
			var button = State.GetButton(Core.Config.Keybinds[keybind]);

			if (callbacks.OnPress is not null) {
				button.OnPress -= callbacks.OnPress;
			}

			if (callbacks.OnHeldRepeat is not null) {
				button.OnHeldRepeat -= callbacks.OnHeldRepeat;
			}

			if (callbacks.OnRelease is not null) {
				button.OnRelease -= callbacks.OnRelease;
			}
		}
	}

	// TODO: Create a config event that calls this whenever there's a keybind change.
	private static void ReloadKeybinds() {
		UnregisterListeners();
		RegisterListeners();
	}

	// Reserved for checking boolean inputs.
	public static void ApplyInputs() {
		var player = Core.Capi.World.Player?.Entity;
		if (player == null) return;

		float sdz = Core.Config.Tuning["StickDeadzone"];

		bool isSprinting = State.GetButton(Core.Config.Keybinds["Sprint"]).IsActive;

		bool isSneaking = State.GetButton(Core.Config.Keybinds["Sneak"]).IsActive;
		bool isJumping  = State.GetButton(Core.Config.Keybinds["Jump"]).IsActive;

		bool isLeftClicking = State.GetButton(Core.Config.Keybinds["LeftClick"]).IsActive;

		bool isRightClicking = State.GetButton(Core.Config.Keybinds["RightClick"]).IsActive;

		player.Controls.Forward  = State.LeftStick.Y < -sdz;
		player.Controls.Backward = State.LeftStick.Y > sdz;
		player.Controls.Left     = State.LeftStick.X < -sdz;
		player.Controls.Right    = State.LeftStick.X > sdz;

		player.Controls.Sprint = isSprinting;
		player.Controls.Sneak  = isSneaking;
		player.Controls.Jump   = isJumping;

		player.Controls.CtrlKey  = isSprinting;
		player.Controls.ShiftKey = isSneaking;

		if (Core.Capi.World is not ClientMain clientMain) return;

		clientMain.MouseStateRaw.Left  = isLeftClicking;
		clientMain.MouseStateRaw.Right = isRightClicking;

		clientMain.InWorldMouseState.Left  = isLeftClicking;
		clientMain.InWorldMouseState.Right = isRightClicking;
	}

	private static void ToggleInventory() =>
		TriggerHotKey(HotkeyCode.InventoryDialog);

	private static void CharacterDialog() =>
		TriggerHotKey(HotkeyCode.CharacterDialog);

	private static void DropItem() =>
		TriggerHotKey(HotkeyCode.DropItem);

	private static void SelectTool() =>
		TriggerHotKey(HotkeyCode.ToolModeSelect);

	private static void EscapeMenuDialog() =>
		TriggerHotKey(HotkeyCode.EscapeMenuDialog);

	private static void WorldMapDialog() =>
		TriggerHotKey(HotkeyCode.WorldMapDialog);

	private static void ChatDialog() =>
		TriggerHotKey(HotkeyCode.ChatDialog);

	private static void FlipHandSlots() =>
		TriggerHotKey(HotkeyCode.FlipHandSlots);

	private static void LeftClickUp() =>
		OnMouseInput(CenterX, CenterY, EnumMouseButton.Left, false);

	private static void LeftClickDown() =>
		OnMouseInput(CenterX, CenterY, EnumMouseButton.Left, true);

	private static void RightClickUp() =>
		OnMouseInput(CenterX, CenterY, EnumMouseButton.Right, false);

	private static void RightClickDown() =>
		OnMouseInput(CenterX, CenterY, EnumMouseButton.Right, true);

	private static void HotbarRight() =>
		ActiveHotbarSlotNumber = (ActiveHotbarSlotNumber + 1) % HotbarLength;

	private static void HotbarLeft() =>
		ActiveHotbarSlotNumber = (ActiveHotbarSlotNumber - 1 + HotbarLength) % HotbarLength;

}
