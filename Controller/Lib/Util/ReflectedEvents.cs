using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Controller.Lib.Util;

public class ReflectedEvents(ICoreClientAPI api) {

	private static BindingFlags accessFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

	private readonly EventInfo? _mouseDownInfo = api.Event.GetType().GetEvent("MouseUp", accessFlags);
	private readonly EventInfo? _mouseUpInfo = api.Event.GetType().GetEvent("MouseDown", accessFlags);

	public void OnMouseInput(int x, int y, EnumMouseButton btn, bool dirIsDown) {
		if (_mouseDownInfo is null || _mouseUpInfo is null) return;

		EventInfo eventInfo = dirIsDown ? _mouseDownInfo : _mouseUpInfo;
		if (eventInfo is null) return;

		MouseEventDelegate? del =
			(MouseEventDelegate?)api.Event.GetType().GetField(eventInfo.Name, accessFlags)?.GetValue(api.Event);

		del?.Invoke(new MouseEvent(x, y, btn));
	}

}
