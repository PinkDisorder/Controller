using JetBrains.Annotations;
using OpenTK.Windowing.GraphicsLibraryFramework;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Controller.Lib;

namespace Controller;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Core : ModSystem {

	#nullable disable
	[UsedImplicitly(ImplicitUseKindFlags.Access)]
	public static string ModId { get; private set; }
	public static Config Config { get; private set; }
	public static ILogger Logger { get; private set; }

	private ICoreClientAPI Capi { get; set; }
	private State State { get; set; }
	private Controls Controls { get; set; }
	private CameraHandler Camera { get; set; }

	private static long _tickListenerId;

	#nullable restore

	public override void StartPre(ICoreAPI api) {
		ModId  = Mod.Info.ModID;
		Logger = Mod.Logger;
		Config = new Config(api, Mod);
	}

	public override void StartClientSide(ICoreClientAPI api) {
		base.StartClientSide(api);
		Capi = api;

		GLFW.UpdateGamepadMappings(api.Assets.Get($"{ModId}:config/gamecontrollerdb.txt").ToText());

		var harmony = new Harmony("net.vividvoid.controller");
		harmony.PatchAll();

		State    = new State();
		Controls = new Controls(Capi);
		Camera   = new CameraHandler(Capi);

		Capi.Event.RegisterRenderer(State, EnumRenderStage.Before);

		_tickListenerId = Capi.Event.RegisterGameTickListener(
			_ => {
				Controls.ApplyInputs();
				Camera.ApplyRightStickCamera();
			},
			0
		);
	}

	public override void Dispose() {
		Capi.Event.UnregisterGameTickListener(_tickListenerId);
		Capi.Event.UnregisterRenderer(State, EnumRenderStage.Before);
		State.Dispose();
		State    = null;
		Controls = null;
		Camera   = null;
		Config   = null;
		Capi     = null;
		base.Dispose();
	}

}
