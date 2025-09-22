using System;

namespace Controller.Lib.Util;

public class AnalogStick(float x, float y) {
	public float X { get; private set; } = x;

	public float Y { get; private set; } = y;

	public void Update(
		int xAxis,
		int yAxis,
		ReadOnlySpan<float> axes,
		bool invertY = false
	) {
		if (axes.Length <= Math.Max(xAxis, yAxis)) return;

		float x = Math.Abs(axes[xAxis]) < Core.Config.Deadzone ? 0f : axes[xAxis];
		float y = Math.Abs(axes[yAxis]) < Core.Config.Deadzone ? 0f : axes[yAxis];

		bool xDidMove = Math.Abs(X - x) > Core.Config.NoiseThreshold;
		bool yDidMove = Math.Abs(Y - y) > Core.Config.NoiseThreshold;

		if (!xDidMove && !yDidMove) return;
		X = x;
		Y = invertY ? -y : y;
	}
}