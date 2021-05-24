using System;
using System.Threading;
using System.Diagnostics;

namespace InputDrawer {
	class Program {
		static void Main(string[] args) {
			Console.Write("Introduce la sensitividad del mouse: ");
			float sensitivity = float.Parse(Console.ReadLine());
			Input inputManager = new Input();
			Vector mousePos = Vector.Zero;
			while (true) {
				if (inputManager.ControllerConnected) {
					//cntr++;
					//Console.WriteLine(cntr);
					if (inputManager.A) {
						Console.Write("A ");
					}
					if (inputManager.B) {
						Console.Write("B ");
					}
					if (inputManager.X) {
						Console.Write("X ");
					}
					if (inputManager.Y) {
						Console.Write("Y ");
					}

					mousePos += inputManager.RS * sensitivity;
					//VirtualController.ClientToScreen(hWnd, ref mouseRef);
					VirtualController.SetCursorPos((int)mousePos.x, -(int)mousePos.y);

					Console.WriteLine($"Mouse: {mousePos}");
					//Console.WriteLine($"RS: {inputManager.RS}");
					
					if (inputManager.Button != 0)
						Console.WriteLine();
				}
				Thread.Sleep(1000 / 30);
			}			
		}
	}
}
