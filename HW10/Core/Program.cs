namespace HW10
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var input = CM.ConsoleMenu.Ask("Выберите должность", ["Консультант", "Менеджер"]);
            Database.ActiveEmployee = input switch
            {
                1 => new Consultant(),
                2 => new Manager(),
                _ => Database.ActiveEmployee
            };
            List<string> options = ["Просмотреть список клиентов", "Выбрать клиента", "Просмотр данных", "Изменение данных"];
            if (Database.ActiveEmployee?.GetType() == typeof(Manager))
                options.Add("Добавить пользователя");

            while (true)
            {
                var action = CM.ConsoleMenu.Ask("Выберите действие", options.ToArray(), true);
                switch (action)
                {
                    case 1:
                        Console.WriteLine("Список клиентов: \n");
                        foreach (var client in Database.Clients)
                        {
                            Console.WriteLine($"\t{client.Surname} {client.Name} (ID: {client.Id})");
                        }
                        Console.WriteLine();
                        break;
                    case 2:
                        Database.ActiveEmployee?.SelectClient(GetClientIdConsole());
                        break;
                    case 3:
                        if (Database.ActiveEmployee?.ClientId is null)
                        {
                            Console.WriteLine("Клиент не выбран! ");
                            break;
                        }
                        Console.WriteLine("Клиент #" + Database.ActiveEmployee.ClientId + "\n");
                        foreach (var prop in (Database.ActiveEmployee?.GetType() == typeof(Manager)
                                     ? typeof(Manager)
                                     : typeof(Consultant)).GetProperties())
                        {
                            Console.WriteLine($"\t{prop.Name}: {prop.GetValue(Database.ActiveEmployee, null)}");
                        }

                        Console.WriteLine();
                        break;
                    case 4:
                        if (Database.ActiveEmployee?.ClientId is null)
                        {
                            Console.WriteLine("Клиент не выбран! ");
                            break;
                        }
                        var props = (Database.ActiveEmployee.GetType() == typeof(Manager)
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
                    case 5:
                        Console.WriteLine("Введите данные о новом пользователе");
                        var newClient = new Client();
                        Database.ActiveEmployee?.SelectClient(newClient);
                        foreach (var prop in (Database.ActiveEmployee?.GetType() == typeof(Manager)
                                     ? typeof(Manager)
                                     : typeof(Consultant)).GetProperties())
                        {
                            if(prop.Name == "ClientId") 
                                continue;
                            Console.Write($"\t{prop.Name}: ");
                            prop.SetValue(Database.ActiveEmployee, Console.ReadLine());
                        }
                        Database.Clients.Add(((Manager)Database.ActiveEmployee!).GetClient()!);
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
