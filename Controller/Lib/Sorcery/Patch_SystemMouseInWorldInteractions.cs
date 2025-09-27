using JetBrains.Annotations;
using HarmonyLib;
using Vintagestory.Client.NoObf;

// ReSharper disable InconsistentNaming

namespace Controller.Lib.Sorcery;

[HarmonyPatch(typeof(SystemMouseInWorldInteractions))]
public static class Patch_SystemMouseInWorldInteractions {

	[UsedImplicitly]
	[HarmonyPrefix]
	[HarmonyPatch("UpdatePicking")]
	public static bool UpdatePicking_Prefix(SystemMouseInWorldInteractions __instance, float dt) {
		ClientMain? clientMain =
			(ClientMain?)AccessTools.Field(typeof(SystemMouseInWorldInteractions), "game").GetValue(__instance);

		if (clientMain is not null) {
			return !clientMain.MouseGrabbed;
		}

		return false;
	}

}
