using System;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Controller.Lib;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {

	[UsedImplicitly(ImplicitUseKindFlags.Access)]
	public static string ModId { get; private set; }

	public static ConfigData Config { get; private set; }

	private ICoreClientAPI Capi { get; set; }

	private State State { get; set; }
	private Controls Controls { get; set; }
	private CameraHandler Camera { get; set; }

	private static long _tickListenerId;

	public static ILogger Logger { get; set; }

	public override void StartPre(ICoreAPI api) {
		ModId  = Mod.Info.ModID;
		Logger = Mod.Logger;

		try {
			var cfg = api.LoadModConfig<ConfigData>($"{Mod.Info.ModID}.json");

			if (cfg != null) {
				Config = cfg;
			}
			else {
				Mod.Logger.Warning("Config file not found, attempting to create it.");
				Config = new ConfigData();
				api.StoreModConfig(Config, "controller.json");
			}
		}
		catch (Exception e) {
			Mod.Logger.Error("Couldn't create config! Loading default settings.");
			Mod.Logger.Error(e);
			Config = new ConfigData();
		}
	}

	public override void StartClientSide(ICoreClientAPI api) {
		base.StartClientSide(api);
		Capi = api;

		GLFW.UpdateGamepadMappings(api.Assets.Get($"{ModId}:controller/config/gamecontrollerdb.txt").ToText());

		State    = new State(api);
		Controls = new Controls(Capi, State);

		Camera = new CameraHandler(Capi, State);

		Capi.Event.RegisterRenderer(State, EnumRenderStage.Before);


		_tickListenerId = Capi.Event.RegisterGameTickListener(
			dt => {
				Controls.ApplyInputs();
				Camera.ApplyRightStickCamera();
			}
			, 0
		);
	}

	public override void Dispose() {
		Capi.Event.UnregisterGameTickListener(_tickListenerId);
		Capi.Event.UnregisterRenderer(State, EnumRenderStage.Before);
		Controls.Dispose();
		State.Dispose();
		State    = null;
		Controls = null;
		Camera   = null;
		Config   = null;
		Capi     = null;
		base.Dispose();
	}

}
