using System;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.Client.NoObf;

namespace Controller.Lib;

public static class CameraHandler {

	private const float PitchClampMin = (float)(Math.PI / 2);
	private const float PitchClampMax = (float)((Math.PI * 13) / 9);

	private static IClientPlayer? Player => Core.Capi.World.Player;

	private static float? _accumulatedPitch = Core.Capi.World.Player?.CameraPitch;

	private static bool BlockFilter(BlockPos pos, Block? block) {
		if (block == null || ClientSettings.RenderMetaBlocks || block.RenderPass != EnumChunkRenderPass.Meta) return true;
		return block.GetInterface<IMetaBlock>(Core.Capi.World, pos)?.IsSelectable(pos) ?? false;
	}

	private static bool EntityFilter(Entity e) {
		return e.IsInteractable && e.EntityId != Player?.Entity.EntityId;
	}

	public static void ApplyRightStickCamera() {
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

		Player.CameraYaw           = yaw;
		Player.CameraPitch         = pitch;
		Core.Capi.Input.MouseYaw   = yaw;
		Core.Capi.Input.MousePitch = pitch;
		UpdateCurrentSelection(pitch, yaw);
	}

	private static void UpdateCurrentSelection(float pitch, float yaw) {
		if (Player is null) return;

		ClientMain game  = (ClientMain)Core.Capi.World;
		float      range = Player.WorldData.PickingRange;

		Vec3d clientCameraPos  = Player.Entity.Pos.XYZ.Clone().Add(0, Player.Entity.LocalEyePos.Y, 0);
		bool  liquidSelectable = game.LiquidSelectable;


		Player.Entity.PreviousBlockSelection = Player.Entity.BlockSelection?.Position.Copy();

		bool rClickStrictly = !game.InWorldMouseState.Left && game.InWorldMouseState.Right;

		bool selectable = Player.InventoryManager.ActiveHotbarSlot?.Itemstack?.Collectible is {
			LiquidSelectable: true
		};

		if (rClickStrictly && selectable) {
			game.forceLiquidSelectable = true;
		}

		Core.Capi.World.RayTraceForSelection(
			clientCameraPos,
			pitch,
			yaw,
			range,
			ref Player.Entity.BlockSelection,
			ref Player.Entity.EntitySelection,
			BlockFilter,
			EntityFilter
		);

		game.forceLiquidSelectable = liquidSelectable;

		Player.Entity.BlockSelection?.Block.OnBeingLookedAt(
			Player,
			Player.Entity.BlockSelection,
			Player.Entity.PreviousBlockSelection is null
			|| Player.Entity.BlockSelection.Position != Player.Entity.PreviousBlockSelection
		);
	}

}
