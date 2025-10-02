using System;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public class CameraHandler(ICoreClientAPI api) {

	private const float PitchClampMin = (float)(Math.PI / 2);
	private const float PitchClampMax = (float)((Math.PI * 13) / 9);

	private IClientPlayer? Player => api.World.Player;

	private float? _accumulatedPitch = api.World.Player?.CameraPitch;

	private bool BlockFilter(BlockPos pos, Block? block) {
		if (block == null | ClientSettings.RenderMetaBlocks || block?.RenderPass != EnumChunkRenderPass.Meta) return true;
		return block.GetInterface<IMetaBlock>(api.World, pos)?.IsSelectable(pos) ?? false;
	}

	private bool EntityFilter(Entity e) {
		return e.IsInteractable && e.EntityId != Player?.Entity.EntityId;
	}

	public void ApplyRightStickCamera() {
		if (Player?.Entity == null) return;

		// yaw is horizontal
		float yaw = Player.CameraYaw;

		// pitch is vertical
		float pitch = (_accumulatedPitch ??= Player.CameraPitch);

		if (Math.Abs(State.RightStick.X) > Core.Config.Tuning["StickDeadzone"]) {
			yaw = GameMath.Mod(yaw - State.RightStick.X * Core.Config.Tuning["SensitivityYaw"], GameMath.TWOPI);
		}

		if (Math.Abs(State.RightStick.Y) > Core.Config.Tuning["StickDeadzone"]) {
			pitch = Math.Clamp(
				pitch - State.RightStick.Y * Core.Config.Tuning["SensitivityPitch"],
				PitchClampMin,
				PitchClampMax
			);

			_accumulatedPitch = pitch;
		}

		Player.CameraYaw     = yaw;
		Player.CameraPitch   = pitch;
		api.Input.MouseYaw   = yaw;
		api.Input.MousePitch = pitch;
		UpdateCurrentSelection(pitch, yaw);
	}

	private void UpdateCurrentSelection(float pitch, float yaw) {
		// Update target
		if (Player is null) return;
		Vec3d clientCameraPos = Player.Entity.Pos.XYZ.Clone().Add(0, Player.Entity.LocalEyePos.Y, 0);

		float range = Player.WorldData.PickingRange;

		BlockSelection?  blockSel = null;
		EntitySelection? entSel   = null;

		api.World.RayTraceForSelection(
			clientCameraPos,
			pitch,
			yaw,
			range,
			ref blockSel,
			ref entSel,
			BlockFilter,
			EntityFilter
		);

		Player.Entity.BlockSelection  = blockSel;
		Player.Entity.EntitySelection = entSel;


		blockSel?.Block.OnBeingLookedAt(
			Player,
			blockSel,
			Player.Entity.PreviousBlockSelection is null || blockSel.Position != Player.Entity.PreviousBlockSelection
		);
	}

}
