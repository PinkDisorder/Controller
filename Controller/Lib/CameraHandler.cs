using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Controller.Lib.Sorcery;

namespace Controller.Lib;

public class CameraHandler(ICoreClientAPI api, State state) {

	// ReSharper disable once InconsistentNaming
	private readonly NativeWindow Window = new WindowWrapper(api).Native;

	private const float PitchClampMin = (float)(Math.PI / 2);
	private const float PitchClampMax = (float)((Math.PI * 13) / 9);

	private IClientPlayer? Player => api.World.Player;

	private float? _accumulatedPitch = api.World.Player?.CameraPitch;

	public unsafe void ApplyRightStickCamera() {
		if (Player?.Entity == null) return;

		GLFW.SetCursorPos(Window.WindowPtr, 0, 0);

		// yaw is horizontal
		float yaw = Player.CameraYaw;

		// pitch is vertical
		float pitch = (_accumulatedPitch ??= Player.CameraPitch);

		if (Math.Abs(state.RightStick.X) > Core.Config.Tuning["StickDeadzone"]) {
			yaw = GameMath.Mod(yaw - state.RightStick.X * Core.Config.Tuning["SensitivityYaw"], GameMath.TWOPI);
		}

		if (Math.Abs(state.RightStick.Y) > Core.Config.Tuning["StickDeadzone"]) {
			pitch = Math.Clamp(
				pitch - state.RightStick.Y * Core.Config.Tuning["SensitivityPitch"]
				, PitchClampMin
				, PitchClampMax
			);

			_accumulatedPitch = pitch;
		}

		Player.CameraYaw     = yaw;
		Player.CameraPitch   = pitch;
		api.Input.MouseYaw   = yaw;
		api.Input.MousePitch = pitch;

		// Update target
		Vec3d clientCameraPos = Player.Entity.Pos.XYZ.Clone().Add(0, Player.Entity.LocalEyePos.Y, 0);

		float range = Player.WorldData.PickingRange;

		BlockSelection?  blockSel = null;
		EntitySelection? entSel   = null;

		api.World.RayTraceForSelection(clientCameraPos, pitch, yaw, range, ref blockSel, ref entSel);

		Player.Entity.BlockSelection  = blockSel;
		Player.Entity.EntitySelection = entSel;

		if (blockSel == null) {
			api.World.HighlightBlocks(Player, 0, [ ]);
			return;
		}

		api.World.HighlightBlocks(Player, 0, [ blockSel.Position ], EnumHighlightBlocksMode.CenteredToSelectedBlock);
	}

}
