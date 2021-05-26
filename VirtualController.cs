using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;

namespace InputDrawer {

    /*
     * RT = B
     * LT = V (Faz),
    */

    public enum Keys {
        //Exclusive to Latin american keyboards
        VK_B = 0x42,
        VK_L = 0x4C,
        VK_D = 0x44,
        VK_X = 0x58,
        VK_U = 0x55,
        VK_E = 0x45,
        VK_H = 0x48,
        VK_V = 0x56
    }

    class VirtualController {

        [Flags]
        public enum MouseEventFlags {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("User32.Dll")]
		public static extern long SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        public static void ClickDown(Vector mousePos) {
            mouse_event((int)MouseEventFlags.LeftDown, (int)mousePos.x, (int)mousePos.y, 0, 0);
        }

        public static void ClickUp(Vector mousePos) {
            mouse_event((int)MouseEventFlags.LeftUp, (int)mousePos.x, (int)mousePos.y, 0, 0);
        }

        public static void SendKey(Keys key) {
            keybd_event((uint)key, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event((uint)key, 0, 0x0002, 0);
		}

    }
}
