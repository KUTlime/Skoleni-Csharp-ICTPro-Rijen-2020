using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Priklad2
{
	class Program
	{
		static void Main()
		{
			ExecuteRun();
		}

		static void ExecuteRun()
		{
			PrintMenu();
			byte i = GetUserInput();
			ExecuteOption(i);
		}


		static void PrintMenu()
		{
			Console.WriteLine("Pro volbu 1, zadejte jedničku.");
			Console.WriteLine("Pro volbu 2, zadejte jedničku.");
			Console.WriteLine("Pro volbu 3, zadejte jedničku.");
			Console.WriteLine("Pro konec programu, zadejte čtyřku.");
		}

		static byte GetUserInput()
		{
			byte returnValue;
			do
			{
				Console.WriteLine("Zadejte svou volbu:");
			} while (byte.TryParse(Console.ReadLine(), out returnValue) == false || ValideUserInput(returnValue) == false);
			return returnValue;
		}

		static void ExecuteOption(in byte i)
		{
			switch (i)
			{
				case 1:
				case 2:
				case 3:
					DoSomeWork(i);
					ExecuteRun();
					break;
				case 4:
					Console.WriteLine("Konec programu");
					break;
				default:
					Console.WriteLine("Zadána neplatná vstupní metoda menu.");
					break;
			}
		}

		static void DoSomeWork(in byte i)
		{
			Console.WriteLine("Zadali jste volbu {0}", i);
		}

		static bool ValideUserInput(byte value)
		{
			return value > 0 && value < 5;
		}

	}


	class EasySolution
	{
		public static void Solution()
		{
			byte i;
			do
			{
				PrintMenu();
				byte.TryParse(Console.ReadLine(), out i);
				ExecuteOption(i);
			} while (i != 4);
		}

		private static void ExecuteOption(byte i)
		{
			switch (i)
			{
				case 1:
				case 2:
				case 3:
					Console.WriteLine("Zadali jste volbu: " + i);
					break;
				case 4:
					Console.WriteLine("Konec programu.");
					break;
				default:
					Console.WriteLine("Neplatná hodnota. Opakujte zadání.");
					break;
			}
		}

		private static void PrintMenu()
		{
			Console.WriteLine("Pro volbu 1, zadejte jedničku.");
			Console.WriteLine("Pro volbu 2, zadejte jedničku.");
			Console.WriteLine("Pro volbu 3, zadejte jedničku.");
			Console.WriteLine("Pro konec programu, zadejte čtyřku.");
			Console.WriteLine("Zadejte svoji volbu:");
		}
	}

}
