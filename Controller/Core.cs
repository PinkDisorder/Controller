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

	public static InputHandler ClientInputHandler { get; set; }

	private readonly InputMonitor _inputMonitor = new(Logger);

	// public override void StartServerSide(ICoreServerAPI api) {
	// 	base.StartServerSide(api);
	// 	Sapi = api;
	// }

	public override void StartClientSide(ICoreClientAPI api) {
		Logger = Mod.Logger;
		Capi = api;
		ModId = Mod.Info.ModID;

		base.StartClientSide(api);
		ClientInputHandler = new InputHandler(api);

		Capi.Event.RegisterRenderer(_inputMonitor, EnumRenderStage.Opaque);

		_inputMonitor.OnStickUpdate += ClientInputHandler.OnStickUpdateHandler;

		_inputMonitor.OnButtonDown += ClientInputHandler.OnButtonDownHandler;

		_inputMonitor.OnButtonUp += ClientInputHandler.OnButtonUpHandler;
		Capi.Event.RegisterGameTickListener(dt => {
			ClientInputHandler.ApplyInputs();
			ClientInputHandler.ApplyRightStickCamera();
		}, 0);
	}

	// EntityAgent class is what handles movement
}