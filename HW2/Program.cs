namespace HW2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fullName = "Ivan";
            string email = "ivan33@mail.ru";

            int codingPts = 80;
            int mathPts = 70;
            int physicsPts = 90;

            byte age = 21;

            Console.WriteLine($"Имя: {fullName}\nПочта: {email}\nВозраст: {age}\n\n" +
                $"Баллы по физике: {physicsPts}\nБаллы по программированию: {codingPts}\nБаллы по математике: {mathPts}\n\n\n");
            Console.ReadKey();

            // task2

            int sum = codingPts + mathPts + physicsPts;
            float avg = sum / 3;


            Console.WriteLine($"Всего баллов: {sum}\nСредний балл: {avg}\n");
            Console.ReadKey();
        }
    }
}
