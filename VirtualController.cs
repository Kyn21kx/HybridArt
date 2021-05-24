using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InputDrawer {

	/// <summary>
	/// Necessary struct for the ClientToScreen function
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {
		public int x, y;

		public POINT(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	class VirtualController {

		[DllImport("User32.Dll")]
		public static extern long SetCursorPos(int x, int y);

		[DllImport("User32.Dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

	}
}
