using System.Collections.Generic;
using Controller.Enums;
using JetBrains.Annotations;

namespace Controller.Lib;

public static class State {
	public static readonly Dictionary<Button, ButtonInput> Buttons = new() {
		{ Button.X, new ButtonInput() },
		{ Button.A, new ButtonInput() },
		{ Button.B, new ButtonInput() },
		{ Button.Y, new ButtonInput() },

		{ Button.Rb, new ButtonInput() },
		{ Button.Lb, new ButtonInput() },

		{ Button.Rt, new ButtonInput() },
		{ Button.Lt, new ButtonInput() },

		{ Button.R3, new ButtonInput() },
		{ Button.L3, new ButtonInput() },

		{ Button.Start, new ButtonInput() },
		{ Button.Back, new ButtonInput() },

		{ Button.DPadUp, new ButtonInput() },
		{ Button.DPadRight, new ButtonInput() },
		{ Button.DPadDown, new ButtonInput() },
		{ Button.DPadLeft, new ButtonInput() },

		{ Button.Guide, new ButtonInput() },
		// PS5 specific
		{ Button.TouchpadClick, new ButtonInput() },
		{ Button.Mute, new ButtonInput() },
		// Steam Deck specific
		// TODO: Get the input codes for these.
		// { Button.L4, new ButtonInput()},
		// { Button.R4, new ButtonInput()},
		// { Button.L5, new ButtonInput()},
		// { Button.R5, new ButtonInput()}
	};

	public class ButtonInput(int heldThreshold = 30) {
		private int HeldCounter { get; set; }

		public bool IsPressed => HeldCounter > 0;
		public bool IsHeld => HeldCounter > heldThreshold;

		public void OnPress() {
			// No point in exceeding this.
			if (HeldCounter > heldThreshold) return;
			HeldCounter++;
		}

		public void OnRelease() {
			HeldCounter = 0;
		}
	}
}