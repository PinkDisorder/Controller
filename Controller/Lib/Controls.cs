using Controller.Lib.Util;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class Controls {

	private readonly ICoreClientAPI _api;
	private readonly State _state;
	private readonly EventCallbacks _callbacks;

	private Button inventory;
	private Button switchHands;
	private Button selectTool;
	private Button characterPanel;
	private Button dropItem;
	private Button menu;
	private Button chat;
	private Button hotbarLeft;
	private Button hotbarRight;
	private Button leftClick;
	private Button rightClick;

	public Controls(ICoreClientAPI api) {
		_api           = api;
		_callbacks     = new EventCallbacks(api);
		inventory      = State.GetButton(Core.Config.Keybinds["Inventory"]);
		switchHands    = State.GetButton(Core.Config.Keybinds["SwitchHands"]);
		selectTool     = State.GetButton(Core.Config.Keybinds["SelectTool"]);
		characterPanel = State.GetButton(Core.Config.Keybinds["CharacterPanel"]);
		dropItem       = State.GetButton(Core.Config.Keybinds["DropItem"]);
		menu           = State.GetButton(Core.Config.Keybinds["Menu"]);
		chat           = State.GetButton(Core.Config.Keybinds["Chat"]);
		hotbarLeft     = State.GetButton(Core.Config.Keybinds["HotbarLeft"]);
		hotbarRight    = State.GetButton(Core.Config.Keybinds["HotbarRight"]);
		leftClick      = State.GetButton(Core.Config.Keybinds["LeftClick"]);
		rightClick     = State.GetButton(Core.Config.Keybinds["RightClick"]);
		RegisterListeners();
	}

	private void RegisterListeners() {
		inventory.OnPress        += _callbacks.ToggleInventory;
		switchHands.OnPress      += _callbacks.FlipHandSlots;
		selectTool.OnPress       += _callbacks.SelectTool;
		characterPanel.OnPress   += _callbacks.CharacterDialog;
		dropItem.OnPress         += _callbacks.DropItem;
		menu.OnPress             += _callbacks.EscapeMenuDialog;
		chat.OnPress             += _callbacks.ChatDialog;
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

		bool isSprinting = State.GetButton(Core.Config.Keybinds["Sprint"]).IsActive;

		bool isSneaking = State.GetButton(Core.Config.Keybinds["Sneak"]).IsActive;
		bool isJumping  = State.GetButton(Core.Config.Keybinds["Jump"]).IsActive;

		bool isLeftClicking =
			State.GetButton(Core.Config.Keybinds["LeftClick"]).IsActive;

		bool isRightClicking =
			State.GetButton(Core.Config.Keybinds["RightClick"]).IsActive;

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
