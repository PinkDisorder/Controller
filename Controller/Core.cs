using Controller.Config;
// using HarmonyLib;
// using Vintagestory.API.Config;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;


namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {
	private static ILogger Logger { get; set; }

	public static string ModId { get; private set; }
	private static ICoreClientAPI Capi { get; set; }
	private static ICoreServerAPI Sapi { get; set; }

	public static ModConfig Config => ConfigLoader.Config;

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
		var handler = new InputHandler(api);

		Capi.Event.RegisterRenderer(_inputMonitor, EnumRenderStage.Opaque);

		_inputMonitor.OnButtonDown += (i, i1) => { Logger.Chat($"Pressed {(Input.Button)i1}"); };

		_inputMonitor.OnTriggerUpdate += (jid, trigger, amount) => { Logger.Chat($"Pressed {trigger} {amount}"); };

		_inputMonitor.OnStickUpdate += ((i, stick, x, y) => { Logger.Chat($"Moved {stick} to x: {x}  y: {y}"); });

		_inputMonitor.OnButtonDown += handler.OnButtonDownHandler;

		_inputMonitor.OnButtonUp += handler.OnButtonUpHandler;
		Capi.Event.RegisterGameTickListener(dt => handler.ApplyInputs(), 0);

	}

	// EntityAgent class is what handles movement

}