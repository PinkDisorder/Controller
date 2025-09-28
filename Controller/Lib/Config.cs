using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class Config {

	private readonly ConfigData Data;
	public Dictionary<string, string> Keybinds => Data.Keybinds;
	public Dictionary<string, float> Tuning => Data.Tuning;

	private readonly ICoreAPI _api;
	private readonly Mod _mod;

	private void TryWriteConfig(ConfigData data) {
		try {
			_api.StoreModConfig(data, $"{_mod.Info.ModID}.json");
		}
		catch (Exception e) {
			_mod.Logger.Error("Config file could not be written");
			_mod.Logger.Fatal(e);
		}
	}

	private ConfigData TryReadConfig() {
		try {
			var data = _api.LoadModConfig<ConfigData>($"{_mod.Info.ModID}.json");
			return data ?? GetDefault();
		}
		catch (Exception e) {
			_mod.Logger.Error("Config file could not be read");
			_mod.Logger.Error(e);

			return GetDefault();
		}

		ConfigData GetDefault() {
			var defaultData = new ConfigData();
			TryWriteConfig(defaultData);
			return defaultData;
		}
	}

	public Config(ICoreAPI api, Mod mod) {
		_mod = mod;
		_api = api;
		Data = TryReadConfig();
	}

}
