
namespace Controller.Enums;
// These were tested on a PS5 controller, need tester for xbox
public enum DualSense {
	X = 0,
	A = 1,
	B = 2,
	Y = 3,

	Lb = 4,
	Rb = 5,

	Lt = 6,
	Rt = 7,

	L3 = 10,
	R3 = 11,

	Back = 8,
	Start = 9,

	Guide = 12,
	TouchpadClick = 13,
	Mute = 14,

	DPadUp = 15,
	DPadRight = 16,
	DPadDown = 17,
	DPadLeft = 18,

	Unknown
}

// TODO: Get correct codes
public enum Xbox {
	X = 0,
	A = 1,
	B = 2,
	Y = 3,

	Lb = 4,
	Rb = 5,

	Lt = 6,
	Rt = 7,

	L3 = 10,
	R3 = 11,

	Back = 8,
	Start = 9,

	Guide = 12,
	TouchpadClick = 13,
	Mute = 14,

	DPadUp = 15,
	DPadRight = 16,
	DPadDown = 17,
	DPadLeft = 18,

	Unknown
}

public enum Switch {
	A = 0,
	B = 1,
	X = 3,
	Y = 4,

	Lb = 6,
	Rb = 7,

	Lt = 8,
	Rt = 9,

	Back = 10, // minus
	Start = 11, // plus
	Guide = 12, // jayu who so kindly reported these said theres an 8bitdo symbol, im guess is the home button

	L3 = 13,
	R3 = 14,

	DPadUp = 20,
	DPadRight = 21,
	DPadDown = 22,
	DPadLeft = 23,
	Unknown
}

public enum Stick {
	Left,
	Right
}

public enum Trigger {
	Lt,
	Rt
}