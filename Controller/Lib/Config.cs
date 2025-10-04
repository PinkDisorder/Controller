using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Enums;
using Vintagestory.API.Common;

namespace Controller.Lib;

public class Config {

	private readonly static string[] ValidButtons = Enum.GetNames(typeof(GamepadButton));

	private readonly ConfigData _data;
	private string[] KnownKeybinds { get; }
	private string[] KnownTuning { get; }

	public Dictionary<string, string> Keybinds => _data.Keybinds;
	public Dictionary<string, float> Tuning => _data.Tuning;

	private ICoreAPI Capi { get; }
	private Mod Mod { get; }

	private ConfigData GetDefault() {
		var defaultData = new ConfigData();
		TryWriteConfig(defaultData);
		return defaultData;
	}

	private bool ValidateConfig(ConfigData data) {
		foreach ((string key, string value) in data.Keybinds) {
			if (!KnownKeybinds.Contains(key) || !ValidButtons.Contains(value)) {
				return false;
			}
		}

		foreach ((string key, float value) in data.Tuning) {
			if (!KnownTuning.Contains(key) || value < 0f || value > 1f) {
				return false;
			}
		}

		return true;
	}

	private void TryWriteConfig(ConfigData data) {
		try {
			Capi.StoreModConfig(data, $"{Mod.Info.ModID}.json");
		} catch (Exception e) {
			Mod.Logger.Error("Config file could not be written.");
			Mod.Logger.Fatal(e);
		}
	}

	private ConfigData TryReadConfig() {
		try {
			var data = Capi.LoadModConfig<ConfigData>($"{Mod.Info.ModID}.json");

			if (data is null) return GetDefault();

			if (ValidateConfig(data)) return data;

			Mod.Logger.Error("Config File is corrupt. Replacing with default.");
			Mod.Logger.Error("Please consult the documentation before making further edits.");
			return GetDefault();
		} catch (Exception e) {
			Mod.Logger.Error("Config file could not be read or was not found.");
			Mod.Logger.Error(e);
			Mod.Logger.Notification("Creating default config file.");
			return GetDefault();
		}
	}

	public void UpdateKeybind(string keybind, string key) {
		if (KnownKeybinds.Contains(keybind) && ValidButtons.Contains(key)) {
			_data.Keybinds[keybind] = key;
			Controls.ReloadKeybinds();
		} else {
			Mod.Logger.Error($"Potentially unknown keybind {keybind} or unknown key ${key}");
		}
	}

	public Config(ICoreAPI api, Mod mod) {
		Mod  = mod;
		Capi = api;
		var defaultData = new ConfigData();
		KnownKeybinds = defaultData.Keybinds.Keys.ToArray();
		KnownTuning   = defaultData.Tuning.Keys.ToArray();
		_data         = TryReadConfig();
	}

}
