using System;
using System.Threading;

namespace InputDrawer {
	class Program {
		static void Main(string[] args) {
			Input inputManager = new Input();
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
					Console.WriteLine($"LS: {inputManager.LS}");
					Console.WriteLine($"RS: {inputManager.RS}");
					if (inputManager.Button != 0)
						Console.WriteLine();
				}
				Thread.Sleep(1000 / 30);
			}			
		}
	}
}
