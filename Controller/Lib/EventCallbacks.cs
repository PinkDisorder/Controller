using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class EventCallbacks(ICoreClientAPI api) {

	private const int HotbarLength = 10;

	private readonly int _centerX = api.Render.FrameWidth / 2;
	private readonly int CenterY = api.Render.FrameHeight / 2;

	private int ActiveHotbarSlotNumber {
		get => api.World.Player?.InventoryManager.ActiveHotbarSlotNumber ?? 0;
		set {
			if (api.World.Player is not null)
				api.World.Player.InventoryManager.ActiveHotbarSlotNumber = value;
		}
	}

	private static BindingFlags accessFlags =
		BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

	private readonly EventInfo? _mouseDownInfo =
		api.Event.GetType().GetEvent("MouseUp", accessFlags);
	private readonly EventInfo? _mouseUpInfo =
		api.Event.GetType().GetEvent("MouseDown", accessFlags);

	private void TriggerHotKey(string hotkeyCode) {
		if (api.World.Player?.Entity == null) return;
		HotKey key = api.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	private void OnMouseInput(int x, int y, EnumMouseButton btn, bool dirIsDown) {
		switch (dirIsDown) {
			case true when _mouseDownInfo is null: return;
			case false when _mouseUpInfo is null:  return;
		}

		EventInfo? eventInfo = dirIsDown ? _mouseDownInfo : _mouseUpInfo;
		if (eventInfo is null) return;

		MouseEventDelegate? del =
			(MouseEventDelegate?)api.Event.GetType()
				.GetField(eventInfo.Name, accessFlags)
				?.GetValue(api.Event);

		del?.Invoke(new MouseEvent(x, y, btn));
	}

	public void ToggleInventory() =>
		TriggerHotKey(HotkeyCode.InventoryDialog);

	public void CharacterDialog() =>
		TriggerHotKey(HotkeyCode.CharacterDialog);

	public void DropItem() =>
		TriggerHotKey(HotkeyCode.DropItem);

	public void SelectTool() =>
		TriggerHotKey(HotkeyCode.ToolModeSelect);

	public void EscapeMenuDialog() =>
		TriggerHotKey(HotkeyCode.EscapeMenuDialog);

	public void ChatDialog() =>
		TriggerHotKey(HotkeyCode.ChatDialog);

	public void FlipHandSlots() =>
		TriggerHotKey(HotkeyCode.FlipHandSlots);

	public void LeftClickUp() =>
		OnMouseInput(_centerX, CenterY, EnumMouseButton.Left, false);

	public void LeftClickDown() =>
		OnMouseInput(_centerX, CenterY, EnumMouseButton.Left, true);

	public void RightClickUp() =>
		OnMouseInput(_centerX, CenterY, EnumMouseButton.Right, false);

	public void RightClickDown() =>
		OnMouseInput(_centerX, CenterY, EnumMouseButton.Right, true);

	public void HotbarRight() =>
		ActiveHotbarSlotNumber = (ActiveHotbarSlotNumber + 1) % HotbarLength;

	public void HotbarLeft() =>
		ActiveHotbarSlotNumber =
			(ActiveHotbarSlotNumber - 1 + HotbarLength) % HotbarLength;

}
