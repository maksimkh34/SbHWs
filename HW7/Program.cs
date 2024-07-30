namespace HW7
{
    internal class Program
    {
        private static void Main()
        {
            const string workerDbPath = "workers.txt";
            var db = new Repository(workerDbPath);

            while (true)
            {
                Console.Write("Меню:\n" +
                              "\n\t1. Показать все записи" +
                              "\n\t2. Показать записи с выборкой по времени" +
                              "\n\t3. Показать запись по ID" +
                              "\n\t4. Добавить новую запись" +
                              "\n\t5. Удалить запись" +
                              "\n\t6. Выход" +
                              "\n\nВвод: ");

                switch (Console.ReadLine())
                {
                    
                    default:
                        return;
                    case "1":
                        Console.Write("\nСортировать по:\n\n" +
                                          "\n\t1. ID" +
                                          "\n\t2. Дате рождения" +
                                          "\n\t3. Дате создания записи" +
                                          "\n\t4. Возрасту" +
                                          "\n\nВвод: ");
                        var entries = Console.ReadLine() switch
                        {
                            "1" => db.ReadEntries().OrderBy(w => w.Id).ToArray(),
                            "2" => db.ReadEntries().OrderBy(w => w.DateOfBirth).ToArray(),
                            "3" => db.ReadEntries().OrderBy(w => w.CreationTime).ToArray(),
                            "4" => db.ReadEntries().OrderBy(w => w.Age).ToArray(),
                            _ => db.ReadEntries().OrderBy(w => w.Id).ToArray()
                        };
                        var empty = true;
                        Console.WriteLine();
                        foreach (var entry in entries)
                        {
                            empty = false;
                            Console.WriteLine(entry.ToStringConsole());
                        }

                        if (empty)
                        {
                            Console.WriteLine("\nЗаписей нет!\n");
                        }
                        break;
                    case "2":
                        Console.Write("Введите дату для начала поиска: ");
                        var from = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("Введите дату для конца поиска: ");
                        var to = DateTime.Parse(Console.ReadLine()!);
                        Console.WriteLine();

                        foreach (var entry in db.ReadEntries(from, to))
                        {
                            Console.WriteLine(entry.ToStringConsole());
                        }

                        break;
                    case "3":
                        Console.Write("Введите номер ID: ");
                        Console.WriteLine();
                        Console.WriteLine(db.GetEntry(int.Parse(Console.ReadLine()!))?.ToStringConsole() ?? "Ошибка получения сотрудника с таким ID.");
                        break;
                    case "4":
                        Console.Write("Введите полное имя: ");
                        var fullName = Console.ReadLine() ?? "indefinited";

                        var createTime = DateTime.Now;

                        Console.Write("Введите возраст: ");
                        var age = Convert.ToByte(Console.ReadLine());

                        Console.Write("Введите рост: ");
                        var growth = Convert.ToByte(Console.ReadLine());

                        Console.Write("Введите дату рождения: ");
                        var dateOfBirth = DateTime.Parse(Console.ReadLine() ?? "");

                        Console.Write("Введите место рождения: ");
                        var placeOfBirth = Console.ReadLine() ?? "indefinited";
                        db.AddEntry(new Worker()
                        {
                            Age = age,
                            CreationTime = createTime,
                            DateOfBirth = dateOfBirth,
                            FullName = fullName,
                            Id = db.GetId(),
                            Growth = growth,
                            PlaceOfBirth = placeOfBirth
                        });
                        Console.WriteLine();
                        break;
                    case "5":
                        Console.Write("Введите номер ID: ");
                        db.RemoveEntry(int.Parse(Console.ReadLine() ?? string.Empty));
                        Console.WriteLine("Готово.");
                        break;

                }
            }
        }
    }

    internal struct Worker
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Age { get; set; }
        public byte Growth { get; set; }

        public override string ToString()
        {
            var w = this;
            var result = typeof(Worker).GetProperties()
                .Aggregate("", (current, propertyInfo) => current + propertyInfo.GetValue(w) + "#");
            return result.Remove(result.Length-1) + "\n";
        }

        public string ToStringConsole()
        {
            return $"Запись {Id} ({CreationTime}):\n\tИмя: {FullName}\n\tМесто рождения: {PlaceOfBirth}\n\tДата рождения: {DateOfBirth:d}\n\tВозраст: {Age}\n\tРост: {Growth}\n";
        }
    }

    internal class Repository
    {
        private const char Delimiter = '#';
        private readonly string _filePath;

        public Repository(string filePath)
        {
            if(!File.Exists(filePath)) File.Create(filePath).Close();
            this._filePath = filePath;
        }

        public Worker[] ReadEntries()
        {
            return File.ReadAllLines(_filePath)
                .Select(line => line.Split(Delimiter))
                .Select(data => new Worker()
                {
                    Age = Convert.ToByte(data[5]),
                    CreationTime = DateTime.Parse(data[3]),
                    DateOfBirth = DateTime.Parse(data[4]),
                    FullName = data[1],
                    Growth = Convert.ToByte(data[6]),
                    Id = Convert.ToInt32(data[0]),
                    PlaceOfBirth = data[2]
                }).ToArray();
        }

        public Worker[] ReadEntries(DateTime from, DateTime to)
        {
            return ReadEntries().Where(worker => worker.CreationTime > from && worker.CreationTime < to).ToArray();
        }

        public Worker? GetEntry(int id)
        {
            var result = ReadEntries().Where(worker => worker.Id == id).ToArray();
            return result.Length == 1 ? result[0] : null;
        }

        public void AddEntry(Worker worker)
        {
            File.AppendAllText(_filePath, worker.ToString());
        }

        public int GetId()
        {
            var id = new Random().Next(10000, 99999);
            return ReadEntries().Select(worker => worker.Id).Contains(id) ? GetId() : id;
        }

        public void RemoveEntry(int id)
        {
            var workers = ReadEntries();
            Worker[]? result = null;
            foreach (var worker in workers)
            {
                if (worker.Id == id)
                {
                    result = workers.Where(w => w.Id != id).ToArray();
                }
            }

            result ??= workers.ToArray();
            File.WriteAllText(_filePath, result.Aggregate("", (current, w) => current + w + "\n"));
        }
    }
}
