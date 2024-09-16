namespace HW12.CM
{
    internal static class ConsoleMenu
    {
        /// <summary>
        /// Функция предназначена для вывода на экран консоли меню с выбором. 
        /// </summary>
        /// <param name="title">Подпись, которая будет отображена при выведении меню</param>
        /// <param name="options">Варианты выбора</param>
        /// <param name="acceptInvalidAnswers">Если значение <see langword="true"/>,
        /// возвращает все значения, которые пользователь ввел,
        /// при этом имеющие значения больше, чем допустимые. Если <see langword="false"/>,
        /// заставляет пользователя сделать выбор от 1 до i-го элемента.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
