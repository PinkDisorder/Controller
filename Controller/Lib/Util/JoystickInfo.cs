using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Controller.Lib.Util;

public class JoystickInfo {

	public readonly int Id;
	public readonly string Name;

	public JoystickInfo() {
		Id   = Enumerable.Range(0, 16).FirstOrDefault(GLFW.JoystickPresent, -1);
		Name = GLFW.GetGamepadName(Id) ?? "None";
	}

}
