using System;
using Controller.Enums;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Controller.Lib;

public class CameraHandler : IRenderer {
	private readonly Vec2f _rightStick = new(0, 0);

	readonly ICoreClientAPI capi;

	// TODO: Put me in the config
	private const float Sensitivity = 0.05f;
	private float _cameraYaw;
	private float _cameraPitch;

	public CameraHandler(ICoreClientAPI api) {
		capi = api;
		IClientPlayer clientPlayer = capi.World.Player;
		if (clientPlayer == null) return;
		_cameraYaw = clientPlayer.CameraYaw; // radians
		_cameraPitch = clientPlayer.CameraPitch; // radians
	}

	public void Dispose() { }
	public double RenderOrder => 0d;
	public int RenderRange => 0;
	public void OnRenderFrame(float deltaTime, EnumRenderStage stage) => ApplyRightStickCamera();

	public void HandleRightStick(int jid, Stick stick, float x, float y) {
		// Y axes need to be inverted on both sticks.
		// Uncertain if this is PS5 controller specific or happens for xbox too.
		// TODO: Config stick axis inverting.
		if (stick != Stick.Right) return;
		_rightStick.X = x;
		_rightStick.Y = y;
	}
	
	private void ApplyRightStickCamera() {
		IClientPlayer clientPlayer = capi.World.Player;
		EntityPlayer entityPlayer = clientPlayer?.Entity;
		if (entityPlayer == null) return;

		// Compute deltas from stick input
		float dx = Math.Abs(_rightStick.X) > InputMonitor.Deadzone ? _rightStick.X * Sensitivity : 0f;
		float dy = Math.Abs(_rightStick.Y) > InputMonitor.Deadzone ? _rightStick.Y * Sensitivity : 0f;

		// Accumulate deltas into authoritative camera state
		_cameraYaw -= dx;
		_cameraPitch -= dy;

		// Clamp pitch to prevent head flip
		const float maxPitchRad = 85f * (float)Math.PI / 180f;
		_cameraPitch = Math.Clamp(_cameraPitch, -maxPitchRad, maxPitchRad);
		
		clientPlayer.CameraYaw = _cameraYaw;
		clientPlayer.CameraPitch = _cameraPitch;
		capi.Input.MouseYaw = _cameraYaw;
		capi.Input.MousePitch = _cameraPitch;

		// Head Yaw is not in Radians
		entityPlayer.Pos.HeadYaw = _cameraYaw;
		entityPlayer.Pos.Yaw = entityPlayer.Pos.HeadYaw;

		// === Block highlighting (unchanged) ===
		Vec3d clientCameraPos = entityPlayer.Pos.XYZ.Clone().Add(0, entityPlayer.LocalEyePos.Y, 0);
		float reach = clientPlayer.WorldData.PickingRange;

		BlockSelection blockSel = null;
		EntitySelection entSel = null;
		capi.World.RayTraceForSelection(clientCameraPos, _cameraPitch, _cameraYaw, reach, ref blockSel, ref entSel);

		entityPlayer.BlockSelection = blockSel;
		entityPlayer.EntitySelection = entSel;

		// Clear previous highlight
		capi.World.HighlightBlocks(capi.World.Player, 0, []);

		if (blockSel == null) return;

		capi.World.HighlightBlocks(
			capi.World.Player,
			0,
			[blockSel.Position],
			EnumHighlightBlocksMode.CenteredToSelectedBlock,
			EnumHighlightShape.Arbitrary
		);
	}
}