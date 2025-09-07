using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Controller.Config;
using Controller.Lib;

namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {
	private static ILogger Logger { get; set; }

	public static string ModId { get; private set; }
	private static ICoreClientAPI Capi { get; set; }
	private static ICoreServerAPI Sapi { get; set; }

	public static ModConfig Config => ConfigLoader.Config;

	// public override void StartServerSide(ICoreServerAPI api) {
	// 	base.StartServerSide(api);
	// 	Sapi = api;
	// }

	public override void StartClientSide(ICoreClientAPI api) {
		Logger = Mod.Logger;
		Capi = api;
		ModId = Mod.Info.ModID;

		base.StartClientSide(api);
		InputMonitor inputMonitor = new(Logger);
		InputHandler input = new(api);
		CameraHandler camera = new(api);

		Capi.Event.RegisterRenderer(inputMonitor, EnumRenderStage.Before);
		Capi.Event.RegisterRenderer(camera, EnumRenderStage.Before);

		inputMonitor.OnStickUpdate += input.HandleLeftStick;
		inputMonitor.OnStickUpdate += camera.HandleRightStick;

		inputMonitor.OnButtonDown += input.HandleButtonDown;
		inputMonitor.OnButtonUp += input.HandleButtonUp;

		Capi.Event.RegisterGameTickListener(dt => { input.ApplyInputs(); }, 0);
	}

	// EntityAgent class is what handles movement
}