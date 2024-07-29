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
                        Console.Write("Введите полное имя: ");
                        var fullName = Console.ReadLine() ?? "indefinited";

                        Console.Write("Введите номер ID: ");
                        var id = Console.ReadLine() ?? "indefinited";

                        var createTime = DateTime.Now;

                        Console.Write("Введите возраст: ");
                        var age = Convert.ToByte(Console.ReadLine());

                        Console.Write("Введите рост: ");
                        var growth = Convert.ToByte(Console.ReadLine());

                        Console.Write("Введите дату рождения: ");
                        var dateOfBirth = DateTime.Parse(Console.ReadLine() ?? "");

                        Console.Write("Введите место рождения: ");
                        var placeOfBirth = Console.ReadLine() ?? "indefinited";

                        Person.AppendPerson(new Person(
                            id, createTime, fullName, age, growth, dateOfBirth, placeOfBirth
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

    internal class Person(string id, DateTime createTime, string fullName, byte age, byte growth, DateTime dateOfBirth,
        string placeOfBirth)
    {
        public string Id = id;
        public DateTime CreateTime = createTime;
        public string FullName = fullName;
        public byte Age = age;
        public byte Growth = growth;
        public DateTime DateOfBirth = dateOfBirth;
        public string PlaceOfBirth = placeOfBirth;

        public static List<Person> Data = ReadData();
        public const string FilePath = "data.txt";
        private const char Delimiter = '#';

        public static string PrintPersons()
        {
            return Data.Aggregate(string.Empty, (current, person) => current + $"Name: {person.FullName}\n" +
                                                                     $"\tEntry created: {person.CreateTime}\n" +
                                                                     $"\tID: {person.Id}\n" +
                                                                     $"\tAge: {person.Age}\n" +
                                                                     $"\tGrowth: {person.Growth}\n" +
                                                                     $"\tDate of birth: {person.DateOfBirth:d}\n" +
                                                                     $"\tPlace of birth: {person.PlaceOfBirth}\n\n");
        }

        public static void AppendPerson(Person person)
        {
            Data.Add(person);
            using var writer = new StreamWriter(FilePath, true);
            writer.WriteLine(person);
        }

        public static List<Person> ReadData()
        {
            try
            {
                using var reader = new StreamReader(FilePath);
                List<Person> result = new();
                while (!reader.EndOfStream)
                {

                    var split = reader.ReadLine()?.Split(Delimiter) ?? Array.Empty<string>();
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
                $"{Id}{Delimiter}" +
                $"{CreateTime}{Delimiter}" +
                $"{fullName}{Delimiter}" +
                $"{Age}{Delimiter}" +
                $"{Growth}{Delimiter}" +
                $"{DateOfBirth}{Delimiter}" +
                $"{PlaceOfBirth}{Delimiter}";
        }
    }
}
