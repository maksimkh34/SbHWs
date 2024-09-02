using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW10.CM
{
    internal static class ConsoleMenu
    {
        public static uint Ask(string title, string[] options, bool acceptInvalidAnswers=false)
        {
            while (true)
            {
                Console.WriteLine(title + "\n");
                for (var i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"\t{i + 1}. {options.ElementAt(i)} ");
                }

                Console.Write("\nВыбор: ");
                var choice = Console.ReadLine();
                Console.WriteLine();
                if (string.IsNullOrEmpty(choice)) 
                    throw new Exception("User input was incorrect");

                uint.TryParse(choice, out var parsed);
                if (parsed != 0)
                {
                    if(parsed <= options.Length || acceptInvalidAnswers)
                        return parsed;
                }

                Console.WriteLine("\nНеправильный выбор!\n");
            }
        }
    }
}
