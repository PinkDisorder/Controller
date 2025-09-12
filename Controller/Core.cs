using System;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Controller.Lib;

namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {
	private static ILogger Logger { get; set; }

	public static ConfigData Config { get; set; }

	[UsedImplicitly] public static string ModId { get; private set; }
	private static ICoreClientAPI Capi { get; set; }

	private static InputMonitor Monitor { get; set; }

	private static InputState State { get; set; }
	
	private static CameraHandler Camera { get; set; }

	private static long _tickListenerId;

	public override void StartPre(ICoreAPI api) {
		try {
			var found = api.LoadModConfig<ConfigData>($"{Mod.Info.ModID}.json");
			if (found != null) {
				Config = found;
			} else {
				Mod.Logger.Warning("Config file could not be loaded, attempting to create it.");
				Config = new ConfigData();
				api.StoreModConfig(Config, "controller.json");
			}
		} catch (Exception e) {
			Mod.Logger.Error("Could neither load nor create config! Loading default settings.");
			Mod.Logger.Error(e);
			Config = new ConfigData();
		}
	}

	public override void StartClientSide(ICoreClientAPI api) {
		Logger = Mod.Logger;
		Capi = api;
		ModId = Mod.Info.ModID;

		base.StartClientSide(api);

		State = new InputState();

		Monitor = new InputMonitor(Logger, State);

		InputHandler input = new(api, State, Monitor.JoyStickId);

		Camera = new CameraHandler(api, State);

		Capi.Event.RegisterRenderer(Monitor, EnumRenderStage.Before);


		_tickListenerId = Capi.Event.RegisterGameTickListener(dt => {
			input.ApplyInputs(dt);
			Camera.ApplyRightStickCamera();
		}, 0);
	}

	public override void Dispose() {
		Capi.Event.UnregisterGameTickListener(_tickListenerId);
		Capi.Event.UnregisterRenderer(Monitor,  EnumRenderStage.Before);
		Camera = null;
		
		Config = null;
		Capi = null;
		base.Dispose();
	}
}