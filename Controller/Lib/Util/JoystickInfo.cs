using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Controller.Lib.Util;

public struct JoystickInfo {
	public readonly int Id;
	public string Name;

	// Except for rY these seem pretty standard, but
	// I've decided to keep them as switches just in case.
	public readonly int LeftAxisX = Core.Config.ControllerType switch { _ => 0 };

	public readonly int LeftAxisY = Core.Config.ControllerType switch { _ => 1 };

	public readonly int RightAxisX = Core.Config.ControllerType switch { _ => 2 };

	public readonly int RightAxisY = Core.Config.ControllerType switch {
		"PS5" => 5, // literally wtf
		_ => 3
	};

	public JoystickInfo() {
		Id = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);
		Name = GLFW.GetGamepadName(Id) ?? "None";
	}
}