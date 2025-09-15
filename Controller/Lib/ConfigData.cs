namespace Controller.Lib;

public class ConfigData {
	public string ControllerType = "PS5";
	
	/**
	 * Treat this like a percentage. 1.00f would be 100%.
	 * 0.15f or 15% is a good catch-all. Older controllers
	 * might need larger numbers. Anything over 0.25f is
	 * most likely overkill.
	 * Even if your controller is brand new, I suggest
	 * keeping the deadzone to at least around 0.10f.
	 */
	public float Deadzone = 0.15f; 
	
	/**
	 * This is the percentage of stick movement that
	 * should be considered noise and ignored.
	 * It's separate from the deadzone because its meant
	 * to ignore imperceptible movements, such as
	 * vibrations caused by simply being held.
	 * Anything over 0.05f is most likely overkill.
	 */
	public float NoiseThreshold = 0.02f;

	// keybinds
	public string Jump = "A";
	public string Sprint = "L3";
	public string Sneak = "R3";

	public string SwitchHands = "B";
	public string SwitchTool = "X";

	public string OpenInventory = "Y";
}