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

	private State State { get; set; }

	public static ICoreClientAPI Capi { get; private set; }

	private long _tickListenerId;

	#nullable restore

	private static void OnGameTick(float v) {
		Controls.ApplyInputs();
		Camera.ApplyRightStickCamera();
	}

	public override void StartPre(ICoreAPI api) {
		ModId  = Mod.Info.ModID;
		Config = new Config(api, Mod);
	}

	public override void StartClientSide(ICoreClientAPI api) {
		base.StartClientSide(api);
		Capi = api;

		// This is required for any modern controller to get read
		// correctly. Remember to update regularly.
		// Perhaps write some kind of method to automatically
		// pull it from git.
		GLFW.UpdateGamepadMappings(api.Assets.Get($"{ModId}:config/gamecontrollerdb.txt").ToText());

		var harmony = new Harmony("net.vividvoid.controller");
		harmony.PatchAll();

		State = new State();

		Capi.Event.RegisterRenderer(State, EnumRenderStage.Before);
		_tickListenerId = Capi.Event.RegisterGameTickListener(OnGameTick, 0);

		Controls.RegisterListeners();
	}

	public override void Dispose() {
		Capi.Event.UnregisterGameTickListener(_tickListenerId);
		Capi.Event.UnregisterRenderer(State, EnumRenderStage.Before);
		Controls.UnregisterListeners();
		State.Dispose();
		State  = null;
		Config = null;
		Capi   = null;
		base.Dispose();
	}

}
