using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Controller.Lib.Sorcery;

namespace Controller.Lib;

public class CameraHandler {

	private readonly ICoreClientAPI capi;
	private readonly State state;

	private readonly NativeWindow Window;

	// TODO: Put me in the config
	private const float SensitivityYaw = 0.05f;
	private const float SensitivityPitch = 0.1f;
	private const float PitchClampMin = (float)(Math.PI / 2);
	private const float PitchClampMax = (float)((Math.PI * 13) / 9);

	private float _accumulatedPitch;

	private readonly IClientPlayer Player;

	public CameraHandler(ICoreClientAPI api, State state) {
		capi       = api;
		this.state = state;

		Window = new WindowWrapper(api).Native;
		Player = capi.World.Player;

		if (Player != null) return;
		_accumulatedPitch = Player.CameraPitch;
	}

	public unsafe void ApplyRightStickCamera() {
		if (Player.Entity == null) return;

		GLFW.SetCursorPos(Window.WindowPtr, 0, 0);

		float yaw = Player.CameraYaw; // X is horizontal camera movement

		if (Math.Abs(state.RightStick.X) > Core.Config.Deadzone) {
			yaw += -state.RightStick.X * SensitivityYaw;
			yaw =  GameMath.Mod(yaw, GameMath.TWOPI);
		}

		float pitch = _accumulatedPitch; // Y is vertical camera movement

		if (Math.Abs(state.RightStick.Y) > Core.Config.Deadzone) {
			pitch             += -state.RightStick.Y * SensitivityPitch;
			pitch             =  Math.Clamp(pitch, PitchClampMin, PitchClampMax);
			_accumulatedPitch =  pitch;
		}

		Player.CameraYaw      = yaw;
		Player.CameraPitch    = pitch;
		capi.Input.MouseYaw   = yaw;
		capi.Input.MousePitch = pitch;


		UpdateTarget(pitch, yaw);
	}

	private void UpdateTarget(float pitch, float yaw) {
		Vec3d clientCameraPos = Player.Entity.Pos.XYZ.Clone().Add(0, Player.Entity.LocalEyePos.Y, 0);

		float range = Player.WorldData.PickingRange;

		BlockSelection  blockSel = null;
		EntitySelection entSel   = null;

		capi.World.RayTraceForSelection(clientCameraPos, pitch, yaw, range, ref blockSel, ref entSel);

		Player.Entity.BlockSelection  = blockSel;
		Player.Entity.EntitySelection = entSel;

		if (blockSel == null) {
			capi.World.HighlightBlocks(Player, 0, [ ]);
			return;
		}

		capi.World.HighlightBlocks(Player, 0, [ blockSel.Position ], EnumHighlightBlocksMode.CenteredToSelectedBlock);
	}

}
