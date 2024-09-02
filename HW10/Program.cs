namespace HW10
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Database.LoadDatabase();
            var input = CM.ConsoleMenu.Ask("Выберите должность", new[] { "Консультант", "Менеджер" });
            Database.ActiveEmployee = input switch
            {
                1 => new Consultant(),
                2 => new Manager(),
                _ => Database.ActiveEmployee
            };
            Database.ActiveEmployee?.SelectClient(GetClientIdConsole());
            List<string> options = new() { "Сменить клиента", "Просмотр данных", "Изменение данных" };
            if (Database.ActiveEmployee?.GetType() == typeof(Manager))
                options.Add("Добавить пользователя");

            while (true)
            {
                var action = CM.ConsoleMenu.Ask("Выберите действие", options.ToArray(), true);
                switch (action)
                {
                    case 1:
                        Database.ActiveEmployee?.SelectClient(GetClientIdConsole());
                        break;
                    case 2:
                        Console.WriteLine("Клиент №" + Database.ActiveEmployee?.ClientId + "\n");
                        foreach (var prop in (Database.ActiveEmployee?.GetType() == typeof(Manager)
                                     ? typeof(Manager)
                                     : typeof(Consultant)).GetProperties())
                        {
                            Console.WriteLine($"\t{prop.Name}: {prop.GetValue(Database.ActiveEmployee, null)}");
                        }

                        Console.WriteLine();
                        break;
                    case 3:
                        var props = (Database.ActiveEmployee?.GetType() == typeof(Manager)
                            ? typeof(Manager)
                            : typeof(Consultant)).GetProperties();
                        var propsNames = props.Select(prop => prop.Name).ToList();
                        var propToChange = props.ElementAt((int)(CM.ConsoleMenu.Ask("Выберите поле для изменения: ", propsNames.ToArray()) -1));
                        Console.Write("Введите новое значение: ");
                        try
                        {
                            switch (propToChange.PropertyType.Name)
                            {
                                case "String":
                                    propToChange.SetValue(Database.ActiveEmployee, Console.ReadLine());
                                    break;
                                case "Nullable`1":
                                {
                                    if (!long.TryParse(Console.ReadLine(), out var value))
                                    {
                                        Console.WriteLine("Введено неверное значение! ");
                                    }

                                    propToChange.SetValue(Database.ActiveEmployee, value);
                                    break;
                                }
                                default:
                                    Console.WriteLine("Неизвестный тип свойства! ");
                                    break;
                            }
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("У вас нет прав на изменение этого поля! \n");
                            break;
                        }
                        Console.WriteLine("Данные изменены! ");
                        break;
                    default:
                        Console.WriteLine("Выход... ");
                        Database.SaveDatabase();
                        return;
                }
            }
        }

        public static long GetClientIdConsole()
        {
            long clientId;
            do
            {
                Console.Write("Введите ID клиента: ");
                long.TryParse(Console.ReadLine(), out clientId);
                Console.WriteLine();
            } while (!Database.ClientIdExists(clientId));
            Console.WriteLine("Клиент найден!\n");
            return clientId;
        }
    }
}
