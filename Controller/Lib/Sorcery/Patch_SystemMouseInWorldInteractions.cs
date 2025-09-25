using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace Controller.Lib.Sorcery;

[HarmonyPatch(typeof(SystemMouseInWorldInteractions))]
public static class Patch_SystemMouseInWorldInteractions {

	[HarmonyPrefix]
	[HarmonyPatch("UpdatePicking")]
	public static bool UpdatePicking_Prefix(SystemMouseInWorldInteractions __instance, float dt) {
		var clientMain = (ClientMain)AccessTools.Field(typeof(SystemMouseInWorldInteractions), "game").GetValue(__instance);

		if (clientMain != null) {
			return !clientMain.MouseGrabbed;
		}

		return false;
	}

}
