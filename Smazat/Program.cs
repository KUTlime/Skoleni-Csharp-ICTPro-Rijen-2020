using System;
using Newtonsoft.Json;

namespace Smazat
{
    class Program
    {
        static void Main()
		{
			// Napište program, který přijde číslo od uživatele 0-9
			// a tento program vypíše slovní podobu tohoto čísla.
			// Výpis formou: Zadali jste nulu
			// Výpis formou: Zadali jste jedničku
			// Výpis formou: Zadali jste dvojku 
			// UPRAVA 1: Upravte program tak, abyste dali uživateli
			// nekonečné možností opravy vstupu.

			byte i = ExtractNumber();
			WriteNumber(ReturnTextNumber(in i));
		}

		public static byte ExtractNumber()
		{
			byte i;
			do
			{
				Console.WriteLine("Zadejte číslo od 0 do 9");
			} while (byte.TryParse(Console.ReadLine(), out i) == false || i > 9);
			return i;
		}

		private static string ReturnTextNumber(in byte i)
		{
			switch (i)
			{
				case 0:
					return "nula";
				case 1:
					return "jednička";
				case 2:
					return "dvojka";
				default:
					return "neznámá hodnota");
			}
		}

		static void WriteNumber(string number)
		{
			Console.WriteLine("Zadali jste " + number);
		}
    }
}
