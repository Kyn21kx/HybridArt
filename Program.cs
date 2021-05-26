using System;
using System.Threading;
using System.IO;
using System.Text;

namespace InputDrawer {
	class Program {

		private enum Instructions {
			A, X, Y, B, RB, LB, RT, LT
		}

		#region Variables
		private const int FRAME_RATE = 1000 / 30;
		
		private static Input inputManager;
		
		private static Vector mousePos;
		private static Vector prevMousePos;
		private static Vector upperLeftCorner;
		private static Vector bottomRightCorner;
		
		private static float sensitivity;
		private static bool clicking;
		private static ulong frame;
		#endregion

		static void Main(string[] args) {
			MENU:
			Console.WriteLine("\tElige la tare que se deberá ejecutar:");
			Console.WriteLine("A) Grabar una sesión");
			Console.WriteLine("B) Ejecutar una sesión");
			Console.WriteLine("C) Salir");
			ConsoleKey k = Console.ReadKey(true).Key;
			switch (k) {
				case ConsoleKey.A:
					Console.Write("Da un nombre a la sesión: ");
					string path = Console.ReadLine();
					Console.WriteLine("Iniciando sesión...");
					InitializeSession();
					Calibrate();
					WriteToFile($"./{path}.hy");
					break;

				case ConsoleKey.B:
					Console.Write("Entra el path de la sesión: ");
					string fileName = Console.ReadLine();
					ExecuteSession($"{fileName}.hy");
					break;

				case ConsoleKey.C:
					return;

				default:
					Console.WriteLine("Opción inválida, presiona cualquier tecla para continuar...");
					Console.ReadKey(true);
					Console.Clear();
					goto MENU;
			}
		}

		private static void InitializeSession() {
			Console.Write("Introduce la sensitividad del mouse: ");
			sensitivity = float.Parse(Console.ReadLine());
			inputManager = new Input();
			mousePos = Vector.Zero;
			upperLeftCorner = Vector.Zero;
			bottomRightCorner = Vector.Zero;
			prevMousePos = mousePos;
			clicking = false;
			frame = 0;
		}

		private static void Calibrate() {
			Console.WriteLine("Coloca el mouse con el joystick en los límites de la imagen y " +
				"presiona A para confirmar la esquina izquierda superior, y X para confirmar la esquina derecha inferior");
			while (upperLeftCorner == Vector.Zero || bottomRightCorner == Vector.Zero) {
				mousePos += (inputManager.RS + inputManager.LS) * sensitivity;
				//Console.WriteLine($"{mousePos}");
				if (inputManager.A && upperLeftCorner == Vector.Zero) {
					Console.WriteLine("Esquina izquierda superior lista");
					upperLeftCorner = mousePos;
					Thread.Sleep(100);
				}
				if (inputManager.X && bottomRightCorner == Vector.Zero) {
					Console.WriteLine("Esquina derecha inferior lista");
					bottomRightCorner = mousePos;
					Thread.Sleep(100);
				}
				VirtualController.SetCursorPos((int)mousePos.x, -(int)mousePos.y);
				Thread.Sleep(FRAME_RATE);
			}
		}

		private static void WriteToFile(string fileName) {
			mousePos = Vector.Zero;
			while (true) {
				if (!inputManager.ControllerConnected) {
					Console.WriteLine("The controller is disconnected!");
					return;
				}

				prevMousePos = mousePos;
				mousePos += (inputManager.RS + inputManager.LS) * sensitivity;
				bool changed = HasChanged(ref mousePos, ref prevMousePos);
				
				if (changed) {
					string frameInfo = GetLine();
					File.AppendAllText(fileName, frameInfo);
					Console.Write("#");
				}
				frame++;
				Thread.Sleep(FRAME_RATE);
			}
		}

		private static string GetLine() {
			StringBuilder outputBuilder = new StringBuilder($"{frame}\t");
			if (inputManager.B) {
				outputBuilder.Append("B\t");
			}
			if (inputManager.X) {
				outputBuilder.Append("X\t");
				clicking = true;
			}
			else if (clicking) {
				clicking = false;
			}
			if (inputManager.Y) {
				outputBuilder.Append("Y\t");
			}
			if (inputManager.RT > 0) {
				outputBuilder.Append($"RT\t");
			}
			if (inputManager.LT > 0) {
				outputBuilder.Append($"LT\t");
			}
			if (inputManager.A) {
				outputBuilder.Append("A\t");
			}
			if (inputManager.RB) {
				outputBuilder.Append("RB\t");
			}
			if (inputManager.LB) {
				outputBuilder.Append("LB\t");
			}
			Vector.Clamp(ref mousePos, upperLeftCorner, bottomRightCorner);
			outputBuilder.Append($"{mousePos}\n");
			
			return outputBuilder.ToString();
		}

		private static bool HasChanged(ref Vector current, ref Vector previous) {
			return inputManager.Button != 0 || current != previous || inputManager.RT > 0;
		}

		private static void ExecuteSession(string fileName) {

			StreamReader sessionReader = new StreamReader(fileName);
			inputManager = new Input();
			string buffer = sessionReader.ReadLine();
			bool changedColor = false;
			clicking = false;
			frame = 0;

			while (buffer != null) {
				if (!inputManager.ControllerConnected) return;
				string[] data = buffer.Split("\t");
				if (frame != ulong.Parse(data[0])) goto NEXT;
				//Get the vector's data
				mousePos = Vector.Parse(data[data.Length - 1]);
				Console.Write($"{mousePos}\t");
				//Set the cursor's position
				VirtualController.SetCursorPos((int)mousePos.x, -(int)mousePos.y);

				//Next, we check for every other piece of information in the string
				for (int i = 1; i < data.Length - 1; i++) {
					//Get the enum value of the string
					Console.Write($"{data[i]}\t");
					Instructions cmd = (Instructions)Enum.Parse(typeof(Instructions), data[i]);

					if (cmd == Instructions.A && !changedColor) {
						changedColor = true;
						VirtualController.SendKey(Keys.VK_X);
					}
					else if (changedColor) {
						changedColor = false;
					}

					if (cmd == Instructions.X) {
						clicking = true;
						VirtualController.ClickDown(mousePos);
					}
					else if (clicking) {
						VirtualController.ClickUp(mousePos);
					}

					switch (cmd) {
						case Instructions.Y:
							VirtualController.SendKey(Keys.VK_D);
							break;
						case Instructions.B:
							VirtualController.SendKey(Keys.VK_L);
							break;
						case Instructions.RB:
							VirtualController.SendKey(Keys.VK_U);
							break;
						case Instructions.LB:
							VirtualController.SendKey(Keys.VK_E);
							break;
						case Instructions.RT:
							VirtualController.SendKey(Keys.VK_B);
							break;
						case Instructions.LT:
							VirtualController.SendKey(Keys.VK_V);
							break;
						default:
							break;
					}
				}
				Console.WriteLine();
				buffer = sessionReader.ReadLine();
				NEXT:
				frame++;
				Thread.Sleep(100);
			}
		}
	}
}
