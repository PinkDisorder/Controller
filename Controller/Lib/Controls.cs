using Controller.Lib.Util;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class Controls {

	private readonly ICoreClientAPI _api;
	private readonly EventCallbacks _callbacks;

	private Button _inventory;
	private Button _switchHands;
	private Button _selectTool;
	private Button _characterPanel;
	private Button _dropItem;
	private Button _menu;
	private Button _chat;
	private Button _hotbarLeft;
	private Button _hotbarRight;
	private Button _leftClick;
	private Button _rightClick;

	public Controls(ICoreClientAPI api) {
		_api            = api;
		_callbacks      = new EventCallbacks(api);
		_inventory      = State.GetButton(Core.Config.Keybinds["Inventory"]);
		_switchHands    = State.GetButton(Core.Config.Keybinds["SwitchHands"]);
		_selectTool     = State.GetButton(Core.Config.Keybinds["SelectTool"]);
		_characterPanel = State.GetButton(Core.Config.Keybinds["CharacterPanel"]);
		_dropItem       = State.GetButton(Core.Config.Keybinds["DropItem"]);
		_menu           = State.GetButton(Core.Config.Keybinds["Menu"]);
		_chat           = State.GetButton(Core.Config.Keybinds["Chat"]);
		_hotbarLeft     = State.GetButton(Core.Config.Keybinds["HotbarLeft"]);
		_hotbarRight    = State.GetButton(Core.Config.Keybinds["HotbarRight"]);
		_leftClick      = State.GetButton(Core.Config.Keybinds["LeftClick"]);
		_rightClick     = State.GetButton(Core.Config.Keybinds["RightClick"]);
		RegisterListeners();
	}

	// TODO: Create a config event that calls this whenever there's a keybind change.
	private void ReloadKeybinds() {
		UnregisterListeners();
		ReInitButtons();
		RegisterListeners();
	}

	private void ReInitButtons() {
		_inventory      = State.GetButton(Core.Config.Keybinds["Inventory"]);
		_switchHands    = State.GetButton(Core.Config.Keybinds["SwitchHands"]);
		_selectTool     = State.GetButton(Core.Config.Keybinds["SelectTool"]);
		_characterPanel = State.GetButton(Core.Config.Keybinds["CharacterPanel"]);
		_dropItem       = State.GetButton(Core.Config.Keybinds["DropItem"]);
		_menu           = State.GetButton(Core.Config.Keybinds["Menu"]);
		_chat           = State.GetButton(Core.Config.Keybinds["Chat"]);
		_hotbarLeft     = State.GetButton(Core.Config.Keybinds["HotbarLeft"]);
		_hotbarRight    = State.GetButton(Core.Config.Keybinds["HotbarRight"]);
		_leftClick      = State.GetButton(Core.Config.Keybinds["LeftClick"]);
		_rightClick     = State.GetButton(Core.Config.Keybinds["RightClick"]);
	}

	private void RegisterListeners() {
		_inventory.OnPress      += _callbacks.ToggleInventory;
		_switchHands.OnPress    += _callbacks.FlipHandSlots;
		_selectTool.OnPress     += _callbacks.SelectTool;
		_characterPanel.OnPress += _callbacks.CharacterDialog;
		_dropItem.OnPress       += _callbacks.DropItem;
		_menu.OnPress           += _callbacks.EscapeMenuDialog;
		_chat.OnPress           += _callbacks.ChatDialog;

		_hotbarLeft.OnPress      += _callbacks.HotbarLeft;
		_hotbarLeft.OnHeldRepeat += _callbacks.HotbarLeft;

		_hotbarRight.OnPress      += _callbacks.HotbarRight;
		_hotbarRight.OnHeldRepeat += _callbacks.HotbarRight;

		_leftClick.OnPress      += _callbacks.LeftClickDown;
		_leftClick.OnHeldRepeat += _callbacks.LeftClickDown;
		_leftClick.OnRelease    += _callbacks.LeftClickUp;

		_rightClick.OnPress     += _callbacks.RightClickDown;
		_rightClick.OnLongPress += _callbacks.RightClickDown;
		_rightClick.OnRelease   += _callbacks.RightClickUp;
	}

	private void UnregisterListeners() {
		_inventory.OnPress      -= _callbacks.ToggleInventory;
		_switchHands.OnPress    -= _callbacks.FlipHandSlots;
		_selectTool.OnPress     -= _callbacks.SelectTool;
		_characterPanel.OnPress -= _callbacks.CharacterDialog;
		_dropItem.OnPress       -= _callbacks.DropItem;
		_menu.OnPress           -= _callbacks.EscapeMenuDialog;
		_chat.OnPress           -= _callbacks.ChatDialog;

		_hotbarLeft.OnPress      -= _callbacks.HotbarLeft;
		_hotbarLeft.OnHeldRepeat -= _callbacks.HotbarLeft;

		_hotbarRight.OnPress      -= _callbacks.HotbarRight;
		_hotbarRight.OnHeldRepeat -= _callbacks.HotbarRight;

		_leftClick.OnPress      -= _callbacks.LeftClickDown;
		_leftClick.OnHeldRepeat -= _callbacks.LeftClickDown;
		_leftClick.OnRelease    -= _callbacks.LeftClickUp;

		_rightClick.OnPress     -= _callbacks.RightClickDown;
		_rightClick.OnLongPress -= _callbacks.RightClickDown;
		_rightClick.OnRelease   -= _callbacks.RightClickUp;
	}

	// Reserved for checking boolean inputs.
	public void ApplyInputs() {
		var player = _api.World.Player?.Entity;
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

		if (_api.World is not ClientMain clientMain) return;

		clientMain.MouseStateRaw.Left  = isLeftClicking;
		clientMain.MouseStateRaw.Right = isRightClicking;

		clientMain.InWorldMouseState.Left  = isLeftClicking;
		clientMain.InWorldMouseState.Right = isRightClicking;
	}

}
