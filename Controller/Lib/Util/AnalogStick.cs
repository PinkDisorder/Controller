using System;

namespace Controller.Lib.Util;

public class AnalogStick(int xAxis, int yAxis) {

	private const float NoiseThreshold = 0.02f;

	public float X { get; private set; }

	public float Y { get; private set; }

	public void Update(ReadOnlySpan<float> axes, bool invertY = false) {
		if (axes.Length <= Math.Max(xAxis, yAxis)) return;

		float x = Math.Abs(axes[xAxis]) < Core.Config.Tuning["StickDeadzone"] ? 0f : axes[xAxis];
		float y = Math.Abs(axes[yAxis]) < Core.Config.Tuning["StickDeadzone"] ? 0f : axes[yAxis];

		bool xDidMove = Math.Abs(X - x) > NoiseThreshold;
		bool yDidMove = Math.Abs(Y - y) > NoiseThreshold;

		if (!xDidMove && !yDidMove) return;
		X = x;
		Y = invertY ? -y : y;
	}

}
