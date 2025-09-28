using Controller.Lib.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class EventCallbacks(ICoreClientAPI api, ReflectedEvents reflectedEvents) {

	private const int HotbarLength = 10;

	private int CenterX = api.Render.FrameWidth / 2;
	private int CenterY = api.Render.FrameHeight / 2;

	private int ActiveHotbarSlotNumber {
		get => api.World.Player?.InventoryManager.ActiveHotbarSlotNumber ?? 0;
		set {
			if (api.World.Player is not null) api.World.Player.InventoryManager.ActiveHotbarSlotNumber = value;
		}
	}

	private void TriggerHotKey(string hotkeyCode) {
		if (api.World.Player?.Entity == null) return;
		HotKey key = api.Input.GetHotKeyByCode(hotkeyCode);
		key.Handler(key.CurrentMapping);
	}

	public void ToggleInventory() => TriggerHotKey(HotkeyCode.InventoryDialog);

	public void CharacterDialog() => TriggerHotKey(HotkeyCode.CharacterDialog);

	public void DropItem() => TriggerHotKey(HotkeyCode.DropItem);

	public void SelectTool() => TriggerHotKey(HotkeyCode.ToolModeSelect);

	public void EscapeMenuDialog() => TriggerHotKey(HotkeyCode.EscapeMenuDialog);

	public void ChatDialog() => TriggerHotKey(HotkeyCode.ChatDialog);

	public void FlipHandSlots() => TriggerHotKey(HotkeyCode.FlipHandSlots);

	public void Jump() => TriggerHotKey(HotkeyCode.Jump);

	public void LeftClickUp() => reflectedEvents.OnMouseInput(CenterX, CenterY, EnumMouseButton.Left, false);

	public void LeftClickDown() => reflectedEvents.OnMouseInput(CenterX, CenterY, EnumMouseButton.Left, true);

	public void RightClickUp() => reflectedEvents.OnMouseInput(CenterX, CenterY, EnumMouseButton.Right, false);

	public void RightClickDown() => reflectedEvents.OnMouseInput(CenterX, CenterY, EnumMouseButton.Right, true);

	public void HotbarRight() => ActiveHotbarSlotNumber = (ActiveHotbarSlotNumber + 1) % HotbarLength;

	public void HotbarLeft() => ActiveHotbarSlotNumber = (ActiveHotbarSlotNumber - 1 + HotbarLength) % HotbarLength;

}
