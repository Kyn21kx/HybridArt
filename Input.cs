using System;
using SharpDX.XInput;

namespace InputDrawer {
	class Input {
		//XOR the button with the inverted desired button, and compare

		private Vector lsReference;
		private Vector rsReference;
		private const int DEADBAND = 2500;

		private Controller controller;

		public Gamepad StateGamepad { get { return controller.GetState().Gamepad; } }
		public bool ControllerConnected { get { return controller.IsConnected; } }
		public bool A { get { return (StateGamepad.Buttons & GamepadButtonFlags.A) != 0; } }
		public bool B { get { return (StateGamepad.Buttons & GamepadButtonFlags.B) != 0; } }
		public bool X { get { return (StateGamepad.Buttons & GamepadButtonFlags.X) != 0; } }
		public bool Y { get { return (StateGamepad.Buttons & GamepadButtonFlags.Y) != 0; } }
		public bool DPadRight { get { return (StateGamepad.Buttons & GamepadButtonFlags.DPadRight) != 0; } }
		public bool DPadLeft { get { return (StateGamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0; } }
		public bool DPadUp { get { return (StateGamepad.Buttons & GamepadButtonFlags.DPadUp) != 0; } }
		public bool DPadDown { get { return (StateGamepad.Buttons & GamepadButtonFlags.DPadDown) != 0; } }
		public bool RB { get { return (StateGamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0; } }
		public bool LB { get { return (StateGamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0; } }
		public int Button { get { return (int)StateGamepad.Buttons; } }
		public Vector LS { get { 
				//Avoid allocating memory everytime we call this property
				lsReference.x = (Math.Abs((float)StateGamepad.LeftThumbX) < DEADBAND) ? 0 : (float)StateGamepad.LeftThumbX / short.MinValue * -100;
				lsReference.y = (Math.Abs((float)StateGamepad.LeftThumbY) < DEADBAND) ? 0 : (float)StateGamepad.LeftThumbY / short.MaxValue * 100; ; 
				return lsReference; 
			} }

		public Vector RS {
			get {
				//Avoid allocating memory everytime we call this property
				rsReference.x = (Math.Abs((float)StateGamepad.RightThumbX) < DEADBAND) ? 0 : (float)StateGamepad.RightThumbX / short.MinValue * -100;
				rsReference.y = (Math.Abs((float)StateGamepad.RightThumbY) < DEADBAND) ? 0 : (float)StateGamepad.RightThumbY / short.MaxValue * 100; ;
				return rsReference;
			}
		}
		public byte RT { get { return StateGamepad.RightTrigger; } }
		public byte LT { get { return StateGamepad.LeftTrigger; } }

		//TODO: Make a vector that represents the position
		public Vector DPad { 
			get {

				return Vector.Zero;
			} }

		public Input() {
			controller = new Controller(UserIndex.One);
			lsReference = Vector.Zero;
			rsReference = Vector.Zero;
		}

	}
}
