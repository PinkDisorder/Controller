namespace Controller.Lib.Util;

public class AnalogueStick(float x, float y) {
	public float X = x;
	public float Y = y;

	public void Update(float x, float y) {
		X = x;
		Y = y;
	}
}