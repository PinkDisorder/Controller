using HarmonyLib;
using JetBrains.Annotations;
using Vintagestory.Client.NoObf;

namespace Controller.Lib.Sorcery;

[HarmonyPatch(typeof(SystemMouseInWorldInteractions), "UpdateCurrentSelection")]
public static class Patch_SystemMouseInWorldInteractions_UpdateCurrentSelection {

	[UsedImplicitly]
	[HarmonyPrefix]
	public static bool Prefix() {
		return false;
	}

}
