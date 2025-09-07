using System;
using Controller.Enums;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Controller.Lib;

public class CameraHandler(ICoreClientAPI api) {
	private readonly Vec2f _rightStick = new(0, 0);

	// TODO: Put me in the config
	private const float Sensitivity = 0.05f;
	private float _cameraYaw;
	private float _cameraPitch;

	public void HandleRightStick(int jid, Stick stick, float x, float y) {
		// Y axes need to be inverted on both sticks.
		// Uncertain if this is PS5 controller specific or happens for xbox too.
		// TODO: Config stick axis inverting.
		if (stick != Stick.Right) return;
		_rightStick.X = x;
		_rightStick.Y = y;
	}


	public void ApplyRightStickCamera() {
		IClientPlayer clientPlayer = api.World.Player;
		EntityPlayer entityPlayer = clientPlayer?.Entity;

		if (entityPlayer == null) return;


		float dx = Math.Abs(_rightStick.X) > InputMonitor.Deadzone ? _rightStick.X * Sensitivity : 0f;
		float dy = Math.Abs(_rightStick.Y) > InputMonitor.Deadzone ? _rightStick.Y * Sensitivity : 0f;

		_cameraYaw -= dx;
		_cameraPitch += dy;

		_cameraPitch = Math.Clamp(_cameraPitch, -89f, 89f);

		clientPlayer.CameraYaw = _cameraYaw;
		clientPlayer.CameraPitch = _cameraPitch;

		api.Input.MouseYaw = _cameraYaw;
		api.Input.MousePitch = _cameraPitch;
		
		entityPlayer.ServerPos.Yaw = _cameraYaw;

		// === Make the block highlight follow the right stick ===
		
		Vec3d clientCameraPos = entityPlayer.Pos.XYZ.Clone().Add(0, entityPlayer.LocalEyePos.Y, 0);
		
		float reach = clientPlayer.WorldData.PickingRange;

		BlockSelection blockSel = null;
		EntitySelection entSel = null;
		api.World.RayTraceForSelection(clientCameraPos, _cameraPitch, _cameraYaw, reach, ref blockSel, ref entSel);

		entityPlayer.BlockSelection = blockSel;
		entityPlayer.EntitySelection = entSel;

		//clear previous highlight
		api.World.HighlightBlocks(api.World.Player, 0, []);

		if (blockSel == null) return;

		api.World.HighlightBlocks(
			api.World.Player,
			0,
			[blockSel.Position],
			EnumHighlightBlocksMode.CenteredToSelectedBlock,
			EnumHighlightShape.Arbitrary
		);
	}
}