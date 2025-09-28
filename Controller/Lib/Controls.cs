using Controller.Lib.Util;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class Controls {

	private readonly ICoreClientAPI _api;
	private readonly State _state;
	private readonly EventCallbacks _callbacks;

	public Controls(ICoreClientAPI api, State state) {
		_api       = api;
		_state     = state;
		_callbacks = new EventCallbacks(api);
		RegisterListeners();
	}

	private void RegisterListeners() {
		Button inventory      = _state.GetButton(Core.Config.Keybinds["Inventory"]);
		Button switchHands    = _state.GetButton(Core.Config.Keybinds["SwitchHands"]);
		Button selectTool     = _state.GetButton(Core.Config.Keybinds["SelectTool"]);
		Button characterPanel = _state.GetButton(Core.Config.Keybinds["CharacterPanel"]);
		Button dropItem       = _state.GetButton(Core.Config.Keybinds["DropItem"]);
		Button menu           = _state.GetButton(Core.Config.Keybinds["Menu"]);
		Button chat           = _state.GetButton(Core.Config.Keybinds["Chat"]);
		Button jump           = _state.GetButton(Core.Config.Keybinds["Jump"]);
		Button hotbarLeft     = _state.GetButton(Core.Config.Keybinds["HotbarLeft"]);
		Button hotbarRight    = _state.GetButton(Core.Config.Keybinds["HotbarRight"]);
		Button leftClick      = _state.GetButton(Core.Config.Keybinds["LeftClick"]);
		Button rightClick     = _state.GetButton(Core.Config.Keybinds["RightClick"]);

		inventory.OnPress        += _callbacks.ToggleInventory;
		switchHands.OnPress      += _callbacks.FlipHandSlots;
		selectTool.OnPress       += _callbacks.SelectTool;
		characterPanel.OnPress   += _callbacks.CharacterDialog;
		dropItem.OnPress         += _callbacks.DropItem;
		menu.OnPress             += _callbacks.EscapeMenuDialog;
		chat.OnPress             += _callbacks.ChatDialog;
		jump.OnPress             += _callbacks.Jump;
		jump.OnHeldRepeat        += _callbacks.Jump;
		hotbarLeft.OnPress       += _callbacks.HotbarLeft;
		hotbarLeft.OnHeldRepeat  += _callbacks.HotbarLeft;
		hotbarRight.OnPress      += _callbacks.HotbarRight;
		hotbarRight.OnHeldRepeat += _callbacks.HotbarRight;
		leftClick.OnPress        += _callbacks.LeftClickDown;
		leftClick.OnHeldRepeat   += _callbacks.LeftClickDown;
		leftClick.OnRelease      += _callbacks.LeftClickUp;
		rightClick.OnPress       += _callbacks.RightClickDown;
		rightClick.OnLongPress   += _callbacks.RightClickDown;
		rightClick.OnRelease     += _callbacks.RightClickUp;
	}

	// Reserved for checking boolean inputs.
	public void ApplyInputs() {
		var player = _api.World.Player?.Entity;
		if (player == null) return;

		float sdz = Core.Config.Tuning["StickDeadzone"];

		bool isSprinting = _state.GetButton(Core.Config.Keybinds["Sprint"]).IsActive;
		bool isSneaking  = _state.GetButton(Core.Config.Keybinds["Sneak"]).IsActive;

		bool isLeftClicking  = _state.GetButton(Core.Config.Keybinds["LeftClick"]).IsActive;
		bool isRightClicking = _state.GetButton(Core.Config.Keybinds["RightClick"]).IsActive;

		player.Controls.Forward  = _state.LeftStick.Y < -sdz;
		player.Controls.Backward = _state.LeftStick.Y > sdz;
		player.Controls.Left     = _state.LeftStick.X < -sdz;
		player.Controls.Right    = _state.LeftStick.X > sdz;

		player.Controls.Sprint = isSprinting;
		player.Controls.Sneak  = isSneaking;

		player.Controls.CtrlKey  = isSprinting;
		player.Controls.ShiftKey = isSneaking;

		if (_api.World is not ClientMain clientMain) return;

		clientMain.MouseStateRaw.Left  = isLeftClicking;
		clientMain.MouseStateRaw.Right = isRightClicking;

		clientMain.InWorldMouseState.Left  = isLeftClicking;
		clientMain.InWorldMouseState.Right = isRightClicking;
	}

	public void Dispose() { }

}
