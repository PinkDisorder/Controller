using Vintagestory.API.Client;

namespace Controller;

public class InputHandler(ICoreClientAPI api) {
	private bool _isJumpHeld;
	private bool _isSprintHeld;
	private bool _isSneakHeld;

	private bool _isForwardHeld;
	private bool _isLeftHeld;
	private bool _isRightHeld;
	private bool _isBackHeld;

	private ICoreClientAPI Capi { get; set; } = api;

	// Call me every tick
	public void ApplyInputs() {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;
		
		player.Controls.Jump = _isJumpHeld;
		player.Controls.Sprint = _isSprintHeld;
		player.Controls.ShiftKey = _isSneakHeld;
	
		player.Controls.Forward = _isForwardHeld;
		player.Controls.Left = _isLeftHeld;
		player.Controls.Right = _isRightHeld;
		player.Controls.Backward = _isBackHeld;
	}

	public void OnButtonDownHandler(int jid, int button) {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;
		

		switch ((Input.Button)button) {
			case Input.Button.A:
				_isJumpHeld = true;
				break;

			case Input.Button.B:
				_isSprintHeld = true;
				break;

			case Input.Button.R3:
				_isSneakHeld = true;
				break;
		}
	}

	public void OnButtonUpHandler(int jid, int button) {
		var player = Capi.World.Player?.Entity;
		if (player == null) return;


		switch ((Input.Button)button) {
			case Input.Button.A:
				_isJumpHeld = false;
				break;

			case Input.Button.B:
				_isSprintHeld = false;
				break;

			case Input.Button.R3:
				_isSneakHeld = false;
				break;
		}
	}
}