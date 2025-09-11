using System;
using Controller.Enums;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class CameraHandler {
	private readonly Vec2f _rightStick = new(0, 0);

	readonly ICoreClientAPI capi;

	// TODO: Put me in the config
	private const float SensitivityYaw = 0.05f;
	private const float SensitivityPitch = 0.1f;


	private float _accumulatedPitch;
	private const float PitchClampMin = (float)(Math.PI / 2);
	private const float PitchClampMax = (float)((Math.PI * 3) / 2);

	// Y axes need to be inverted on RS.
	public void HandleRightStick(float x, float y) => (_rightStick.X, _rightStick.Y) = (x, -y);

	public CameraHandler(ICoreClientAPI api) {
		capi = api;
		IClientPlayer clientPlayer = capi.World.Player;
		if (clientPlayer == null) return;

		_accumulatedPitch = clientPlayer.CameraPitch;
	}


	public void ApplyRightStickCamera() {
		IClientPlayer clientPlayer = capi.World.Player;
		EntityPlayer entityPlayer = clientPlayer?.Entity;
		if (entityPlayer == null) return;

		// Get current camera angles
		// Yaw can be read from the clientPlayer but pitch has to be treated
		// in an accumulative manner otherwise everything breaks.
		float yaw = clientPlayer.CameraYaw;
		float pitch = _accumulatedPitch;

		// X is horizontal camera movement
		if (Math.Abs(_rightStick.X) > InputMonitor.Deadzone) {
			yaw += -_rightStick.X * SensitivityYaw;
			yaw = GameMath.Mod(yaw, GameMath.TWOPI);
		}

		// Y is vertical camera movement
		if (Math.Abs(_rightStick.Y) > InputMonitor.Deadzone) {
			pitch += -_rightStick.Y * SensitivityPitch;
			pitch = Math.Clamp(pitch, PitchClampMin, PitchClampMax);
			_accumulatedPitch = pitch;
		}

		// Always update camera and mouse
		clientPlayer.CameraYaw = yaw;
		clientPlayer.CameraPitch = pitch;
		capi.Input.MouseYaw = yaw;
		capi.Input.MousePitch = pitch;

		UpdateBlockTarget(pitch, yaw);
	}

	private void UpdateBlockTarget(float pitch, float yaw) {
		IClientPlayer player = capi.World.Player;
		Vec3d clientCameraPos = player.Entity.Pos.XYZ.Clone().Add(0, player.Entity.LocalEyePos.Y, 0);
		float reach = player.WorldData.PickingRange;

		BlockSelection blockSel = null;
		EntitySelection entSel = null;
		capi.World.RayTraceForSelection(clientCameraPos, pitch, yaw, reach, ref blockSel, ref entSel);

		player.Entity.BlockSelection = blockSel;
		player.Entity.EntitySelection = entSel;
		if (blockSel == null) {
			capi.World.HighlightBlocks(player, 0, []);
			return;
		}

		capi.World.HighlightBlocks(
			player,
			0,
			[blockSel.Position],
			EnumHighlightBlocksMode.CenteredToSelectedBlock
		);
	}
}