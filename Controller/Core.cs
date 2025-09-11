using System;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Controller.Config;
using Controller.Enums;
using Controller.Lib;

namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {
	private static ILogger Logger { get; set; }

	public static string ModId { get; private set; }
	private static ICoreClientAPI Capi { get; set; }

	public static ModConfig Config => ConfigLoader.Config;

	public override void StartClientSide(ICoreClientAPI api) {
		Logger = Mod.Logger;
		Capi = api;
		ModId = Mod.Info.ModID;

		base.StartClientSide(api);

		State state = new();

		InputMonitor inputMonitor = new(Logger);

		InputHandler input = new(api, state, inputMonitor.PrimaryJoystick);

		CameraHandler camera = new(api, state);

		Capi.Event.RegisterRenderer(inputMonitor, EnumRenderStage.Before);

		inputMonitor.OnStickUpdate += (jid, stick, x, y) => {
			switch (stick) {
				case Stick.Left:
					input.HandleLeftStick(x, y);
					break;
				case Stick.Right:
					camera.HandleRightStick(x, y);
					break;
				default:
					Logger.Error(new ArgumentOutOfRangeException(nameof(stick), stick, null));
					break;
			}
		};

		inputMonitor.OnButtonDown += input.HandlePress;
		inputMonitor.OnButtonUp += input.HandleRelease;

		inputMonitor.OnButtonDown += (jid, btn) => Capi.Logger.Debug($"Pressed {btn}");

		Capi.Event.RegisterGameTickListener(dt => {
			input.ApplyInputs();
			camera.ApplyRightStickCamera();
		}, 0);
	}
}