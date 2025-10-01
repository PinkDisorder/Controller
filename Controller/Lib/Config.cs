using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class Config {

	private readonly ConfigData Data;
	private readonly ConfigData DefaultData;
	private string[] KnownKeybinds { get; }
	private string[] KnownTuning { get; }

	public Dictionary<string, string> Keybinds => Data.Keybinds;
	public Dictionary<string, float> Tuning => Data.Tuning;

	private ICoreAPI Capi { get; }
	private Mod Mod { get; }

	private void TryWriteConfig(ConfigData data) {
		try {
			Capi.StoreModConfig(data, $"{Mod.Info.ModID}.json");
		} catch (Exception e) {
			Mod.Logger.Error("Config file could not be written");
			Mod.Logger.Fatal(e);
		}
	}

	private ConfigData GetDefault() {
		var defaultData = new ConfigData();
		TryWriteConfig(defaultData);
		return defaultData;
	}

	private ConfigData TryReadConfig() {
		try {
			var data = Capi.LoadModConfig<ConfigData>($"{Mod.Info.ModID}.json");

			if (data is null) {
				return GetDefault();
			}

			bool satisfy1 = data.Keybinds.Keys.ToArray().SequenceEqual(KnownKeybinds);
			bool satisfy2 = data.Tuning.Keys.ToArray().SequenceEqual(KnownTuning);

			if (satisfy1 && satisfy2) {
				return data;
			}

			Mod.Logger.Error("Config File is corrupt. Attempting to recreate.");
			return GetDefault();
		} catch (Exception e) {
			Mod.Logger.Error("Config file could not be read");
			Mod.Logger.Error(e);
			return GetDefault();
		}
	}

	public Config(ICoreAPI api, Mod mod) {
		Mod           = mod;
		Capi          = api;
		DefaultData   = new ConfigData();
		KnownKeybinds = DefaultData.Keybinds.Keys.ToArray();
		KnownTuning   = DefaultData.Tuning.Keys.ToArray();
		Data          = TryReadConfig();
	}

}
