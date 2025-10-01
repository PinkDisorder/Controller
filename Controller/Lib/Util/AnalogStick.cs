using System;

namespace Controller.Lib.Util;

public class AnalogStick(int xAxis, int yAxis) {

	private const int DecimalsToKeep = 2;

	private const float NoiseThreshold = 0.02f;

	private static float Smoothing => Core.Config.Tuning["StickSmoothing"];

	private static float Deadzone => Core.Config.Tuning["StickDeadzone"];

	public float X { get; private set; }
	public float Y { get; private set; }

	private static float LinearInterpolation(float current, float target, float movementFraction) {
		return current + (target - current) * movementFraction;
	}

	public void Update(ReadOnlySpan<float> axes, bool invertY = false) {
		if (axes.Length <= Math.Max(xAxis, yAxis)) return;

		float rawX = axes[xAxis];
		float rawY = invertY ? -axes[yAxis] : axes[yAxis];

		float xPos = MathF.Abs(rawX) < Deadzone ? 0f : rawX;
		float yPos = MathF.Abs(rawY) < Deadzone ? 0f : rawY;

		float xTarget = MathF.Round(xPos, DecimalsToKeep);
		float yTarget = MathF.Round(yPos, DecimalsToKeep);

		bool xDidMove = MathF.Abs(X - xTarget) >= NoiseThreshold;
		bool yDidMove = MathF.Abs(Y - yTarget) >= NoiseThreshold;

		if (!xDidMove && !yDidMove) return;
		X = LinearInterpolation(X, xTarget, Smoothing);
		Y = LinearInterpolation(Y, yTarget, Smoothing);
	}

}
