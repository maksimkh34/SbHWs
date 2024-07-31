using System.Reflection;

namespace HW6
{
    internal class Program
    {
        private static void Main()
        {
            while(true)
            {
                Console.Write("Number: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine(Person.PrintPersons());
                        break;
                    case "2":
                        List<string?> values = new();
                        Person person = new();

                        foreach (var prop  in person.GetProperties())
                        {
                            if(!prop.AskForInput) continue;
                            Console.Write($"Введите {prop.DisplayableName}: ");
                            values.Add(Console.ReadLine());
                        }

                        Person.AppendPerson(new Person(
                            values[0]!, DateTime.Now, values[1]!, Convert.ToByte(values[2]!), Convert.ToByte(values[3]!), DateTime.Parse(values[4]!), values[5]!
                            ));

                        Console.WriteLine("\nCompleted!");
                        break;

                    case null:
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Unknown command.\n");
                        break;
                }

                Console.WriteLine();
            }
        }
    }

    public class Property(PropertyInfo basePropertyInfo, string displayableName)
    {
        public PropertyInfo BasePropertyInfo = basePropertyInfo;
        public bool AskForInput = true;
        public string DisplayableName = displayableName;

        public object? GetValue(Person person) => BasePropertyInfo.GetValue(person);
    }

    public class Person(string id, DateTime createTime, string fullName, byte age, byte growth, DateTime dateOfBirth,
        string placeOfBirth)
    {
        public string Id { get; set; } = id;
        public DateTime CreateTime { get; set; } = createTime;
        public string FullName { get; set; } = fullName;
        public byte Age { get; set; } = age;
        public byte Growth { get; set; } = growth;
        public DateTime DateOfBirth { get; set; } = dateOfBirth;
        public string PlaceOfBirth { get; set; } = placeOfBirth;

        public Person() : this("", DateTime.Now, "", 1, 1, DateTime.Now, "")
        {}

        public Property[] GetProperties()
        {
            var props = typeof(Person).GetProperties();
            return new Property[]
            {
                new(props[0], "ID"),
                new(props[1], "Дата создания") { AskForInput = false},
                new(props[2], "Полное имя"),
                new(props[3], "Возраст"),
                new(props[4], "Рост"),
                new(props[5], "Дата рождения"),
                new(props[6], "Место рождения"),
            };
        }

        public static List<Person> Data = ReadData();

        public const string FilePath = "data.txt";
        private const char Delimiter = '#';
        private const string DelimiterStrReplace = "%del%";

        public static string PrintPersons()
        {
            var result = "";
            foreach (var person in Data)
            {
                result += "\n";
                result = person.GetProperties().Aggregate(result, (current, prop) => current + $"\t{prop.DisplayableName}: {prop.GetValue(person)}\n");
            }
            return result;
        }

        public static void AppendPerson(Person person)
        {
            Data.Add(person);
            using var writer = new StreamWriter(FilePath, true);
            writer.WriteLine(person.ToString().Replace(Delimiter.ToString(), DelimiterStrReplace));
            writer.Close();
        }

        public static List<Person> ReadData()
        {
            try
            {
                using var reader = new StreamReader(FilePath);
                List<Person> result = new();
                while (!reader.EndOfStream)
                {

                    var split = reader.ReadLine()?.Replace(DelimiterStrReplace, Delimiter.ToString()).Split(Delimiter) ?? Array.Empty<string>();
                    result.Add(new Person(
                        split[0], DateTime.Parse(split[1]), split[2], Convert.ToByte(split[3]),
                        Convert.ToByte(split[4]), DateTime.Parse(split[5]), split[6]
                    ));
                }
                return result;
            }
            catch (FileNotFoundException)
            {
                File.Create(FilePath);
            }

            return new List<Person>();
        }

        public override string ToString()
        {
            return 
                GetProperties().Aggregate("", (current, prop) => current + (prop.GetValue(this)?.ToString() + Delimiter));
        }
    }
}
