namespace HW3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true) { 
                //Task1();
                //Task2();
                //Task3();
                //Task4();
                Task5();
            }

        }
        static void Task1()
        {
            Console.Write("Введите целое число для проверки: ");
            string input = Console.ReadLine()!;
            int num = int.Parse(input);
            Console.WriteLine("Число " + (num % 2 == 0 ? "четное" : "нечетное") + ".");
            Console.ReadKey();
        }

        static void Task2()
        {
            int points = 0;

            Console.Write("Введите кол-во карт: ");
            int counter = int.Parse(Console.ReadLine()!);
            Console.WriteLine();
            for(; counter > 0; counter--) {
                Console.Write("Введите номинал следующей карты: ");
                string input = Console.ReadLine()!;
                // _ = new string[] { "J", "Q", "K", "T" }.Contains(input) ? points += 10 : points += int.Parse(input);
                points += input switch
                {
                    "J" => 10,
                    "Q" => 10,
                    "K" => 10,
                    "T" => 10,
                    "6" => 6,
                    "7" => 7,
                    "8" => 8,
                    "9" => 9,
                    "10" => 10,
                    _ => 0
                };
            }
            Console.WriteLine("Кол-во очков: " + points);
            Console.ReadKey();
        }

        static void Task3()
        {
            Console.Write("Введите число для проверки: ");
            int input = int.Parse(Console.ReadLine()!);
            Console.WriteLine();
            int i = 2;  // На 1 будут делиться все числа...

            bool isSimple = true;
            while(i <= input - 1)
            {
                isSimple = input % i != 0 && isSimple;
                i++;
            }
            if (isSimple) Console.WriteLine("Число простое! ");
            else Console.WriteLine("Число не простое! ");
        }

        static int GetNum()
        {
            Console.Write("Введите число: ");
            return int.Parse(Console.ReadLine()!);
        }

        static void Task4()
        {
            Console.Write("Введите кол-во чисел: ");
            int counter = int.Parse(Console.ReadLine()!) - 1;
            Console.WriteLine();

            int minValue = GetNum();    // Лучше, чем MaxValue
            for(; counter > 0; counter--)
            {
                var inputVaule = GetNum();
                if(inputVaule < minValue) minValue = inputVaule;
            }

            Console.WriteLine("\nМинимальное число: " + minValue);
        }

        static void Task5()
        {
            Console.Write("Введите максимальное значение: ");
            int maxValue = int.Parse(Console.ReadLine()!);
            Console.WriteLine();

            int number = new Random().Next(maxValue + 1);
            int? guess = null;
            while(true)
            {
                try { guess = GetNum(); }
                catch { break; }
                if (guess == number) break;
                else Console.WriteLine("Введенное число " + (guess > number ? "больше" : "меньше") + " загаданного.");
            }
            if(guess == number)
            {
                Console.WriteLine("Поздравляю! Игра выиграна. ");
            }
            else
            {
                Console.WriteLine("Загаданное число было " + number);
                return;
            }
        }
    }
}
