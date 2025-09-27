using System;
using System.Reflection;
using OpenTK.Windowing.Desktop;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace Controller.Lib.Sorcery;

internal sealed class WindowWrapper {

	private readonly GameWindowNative _window;

	public NativeWindow Native => _window;

	private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

	public WindowWrapper(ICoreClientAPI clientApi) {
		FieldInfo? field = typeof(ClientMain).GetField("Platform", Flags);

		ClientPlatformWindows? platform = (ClientPlatformWindows?)field?.GetValue(clientApi.World as ClientMain);

		_window = platform?.window
			?? throw new InvalidOperationException("Unable to get game native window from client api");
	}

}
