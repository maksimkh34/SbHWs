using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace HW8
{
    public static class ListAggregator
    {

        public static List<int> Generate(int min, int max, int length)
        {
            var result = new int[length];
            var rand = new Random();
            for (var i = 0; i < length; i++)
            {
                result[i] = rand.Next(min, max + 1);
            }

            return result.ToList();
        }

        public static void Print<T>(List<T> list)
        {
            Console.WriteLine("{");
            var count = list.Count - 1;
            for (var i = 0; i < count; i++)
            {
                Console.WriteLine($"\t[{i}]: {list[i]},");
            }
            Console.WriteLine($"\t[{count}]: {list[count]}\n" + "}");
        }

        public static void RemoveElements<T>(ref List<T> list, T min, T max) where T : IComparable<T>
        {
            list = list.Where(item => !(item.CompareTo(min) > 0 && item.CompareTo(max) < 0)).ToList();
        }

    }

    internal class Program
    {
        private static void Main()
        {
            Task4();
        }

        public static void Task1()
        {
            var list = ListAggregator.Generate(0, 100, 100);
            Console.WriteLine($"\n\tBefore (len {list.Count})\n");
            ListAggregator.Print(list);
            ListAggregator.RemoveElements(ref list, 25, 50);
            Console.WriteLine($"\n\tAfter (len {list.Count})\n");
            ListAggregator.Print(list);
        }

        public static void Task2()
        {
            Dictionary<long, string> phoneNumbers = new();

            string? input;
            do
            {
                Console.Write("Введите номер телефона: ");
                input = Console.ReadLine();
                if (long.TryParse(input, out var number))
                {
                    Console.Write("Введите фамилию: ");
                    phoneNumbers.Add(number, Console.ReadLine() ?? "-");
                    Console.WriteLine("Запись добавлена! ");
                }
                else
                {
                    if(string.IsNullOrEmpty(input)) continue;
                    Console.WriteLine("Это не похоже на номер телефона...");
                }
            } while(!string.IsNullOrEmpty(input));

            while (true)
            {
                Console.Write("Введите номер телефона для поиска: ");
                input = Console.ReadLine();
                if (long.TryParse(input, out var number))
                {
                    Console.WriteLine(phoneNumbers.TryGetValue(number, out input)
                        ? $"Номером {number} владеет {input}!"
                        : "Такой номер не зарегистрирован!");
                }
                else
                {
                    if (string.IsNullOrEmpty(input)) return;
                    Console.WriteLine("Это не похоже на номер телефона...");
                }
            }
        }

        public static void Task3()
        {
            HashSet<int> set = new();
            string? input;

            do
            {
                Console.Write("Введите число: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out var number))
                {
                    if(set.Contains(number))
                    {
                        Console.WriteLine("Такое число уже есть! ");
                    }
                    else
                    {
                        set.Add(number);
                        Console.WriteLine("Добавлено! ");
                    }
                }
                else
                {
                    Console.WriteLine("Введенное число - не int. ");
                }

            } while (!string.IsNullOrEmpty(input));
            
        }

        public static void Task4()
        {
            Console.Write("Введите ФИО: ");
            var fullName = Console.ReadLine()!;

            Console.Write("Введите адрес: ");
            var street = Console.ReadLine()!;

            Console.Write("Введите номер дома: ");
            var houseNumber = Console.ReadLine()!;

            Console.Write("Введите номер квартиры: ");
            var flatNumber = Console.ReadLine()!;

            Console.Write("Введите номер телефона: ");
            var mobilePhoneNumber = Console.ReadLine()!;

            Console.Write("Введите номер домашнего телефона: ");
            var flatPhoneNumber = Console.ReadLine()!;

            XElement person = new XElement("Person",
                new XAttribute("name", fullName),
                new XElement("Address",
                    new XElement("Street", street),
                    new XElement("HouseNumber", houseNumber),
                    new XElement("FlatNumber", flatNumber)
                ),
                new XElement("Phones",
                    new XElement("MobilePhone", mobilePhoneNumber),
                    new XElement("FlatPhone", flatPhoneNumber)
                )
            );

            if(File.Exists("person.xml")) File.Delete("person.xml");
            person.Save(new FileStream("person.xml", FileMode.Create));
        }
    }
}
