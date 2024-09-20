namespace HW12
{
    public static class Program
    {
        public static readonly List<User> Users = LoadUsers();
        private static int CurrentUserIndex { get; set; }

        public static User CurrentUser
        {
            get => Users[CurrentUserIndex];
            set
            {
                for (var i = 0; i < Users.Count; i++)
                {
                    if (Users[i] == value) CurrentUserIndex = i;
                }
            }
        }

        public static void ConsoleMain()
        {
            if (Users.Count == 0)
            {
                RegisterConsole();
            }
            else
            {
                var logged = false;

                while (!logged)
                {
                    Console.WriteLine("Войдите в систему. \n");
                    Console.Write("Имя: ");
                    var name = Console.ReadLine()!;
                    Console.Write("Фамилия: ");
                    var surname = Console.ReadLine()!;
                    var user = GetUser(name, surname);
                    logged = user != null;
                    if (!logged) Console.WriteLine("\nПользователь не зарегистрирован! \n");
                }
            }
            Console.WriteLine($"Добро пожаловать, {CurrentUser.Name}!");
            while (true)
            {
                switch (CM.ConsoleMenu.Ask($"Выберите опцию ({CurrentUser.Name}): ", [
                        "Зарегистрировать нового пользователя",
                        "Внести сумму", "Снять деньги", "Перевести пользователю",
                        "Открыть депозитный счет", "Снять деньги с депозитного счета",
                        "Вывести информацию", "Разблокировать депозит"
                    ], true))
                {
                    case 1:
                        RegisterConsole();
                        break;
                    case 2:
                        Console.Write("Введите сумму: ");
                        if (!uint.TryParse(Console.ReadLine(), out var amountToDeposit))
                        {
                            Console.WriteLine("Введена неверная сумма. ");
                            break;
                        }
                        CurrentUser.NonDepositAccount.Deposit(amountToDeposit);
                        break;
                    case 3:
                        Console.Write("Введите сумму: ");
                        if (!uint.TryParse(Console.ReadLine(), out var amountToTake))
                        {
                            Console.WriteLine("Введена неверная сумма. ");
                            break;
                        }
                        CurrentUser.NonDepositAccount.Take(amountToTake);
                        break;
                    case 4:
                        Console.WriteLine("Введите данные пользователя, которому хотите перевести средства: \n");
                        Console.Write("Имя: ");
                        var name = Console.ReadLine()!;
                        Console.Write("Фамилия: ");
                        var surname = Console.ReadLine()!;
                        var user = GetUser(name, surname);
                        if (user == null)
                        {
                            Console.WriteLine("Пользователь не найден! ");
                            break;
                        }
                        Console.Write("Введите сумму: ");
                        if (!uint.TryParse(Console.ReadLine(), out var amountToTransfer))
                        {
                            Console.WriteLine("Введена неверная сумма. ");
                            break;
                        }
                        CurrentUser.NonDepositAccount.Transfer(user, amountToTransfer);
                        break;
                    case 5:
                        Console.Write("Введите сумму: ");
                        if (!uint.TryParse(Console.ReadLine(), out var amount))
                        {
                            Console.WriteLine("Введена неверная сумма. ");
                            break;
                        }
                        CurrentUser.DepositAccount.UnblockAccount(true);
                        CurrentUser.DepositAccount.Deposit(amount);
                        CurrentUser.DepositAccount.BlockAccount(true);
                        Console.WriteLine("Сумма внесена! До определенного срока снять ее будет невозможно. ");
                        break;
                    case 6:
                        if (CurrentUser.DepositAccount.IsBlocked() && CurrentUser.DepositAccount.GetBalance() == 0)
                        {
                            Console.WriteLine("Счет не открыт! "); break;
                        }
                        if (CurrentUser.DepositAccount.IsBlocked() && CurrentUser.DepositAccount.GetBalance() != 0)
                        {
                            Console.WriteLine("Срок не вышел! "); break;
                        }
                        CurrentUser.DepositAccount.Take(CurrentUser.DepositAccount.GetBalance());
                        break;
                    case 7:
                        foreach (var usr in Users)
                        {
                            Console.WriteLine($"\nПользователь {usr.Name} {usr.Surname}" +
                                              $"\n\tБаланс: {usr.NonDepositAccount.GetBalance()}" +
                                              $"\n\tДепозит открыт: {(usr.DepositAccount.IsBlocked() && usr.DepositAccount.GetBalance() == 0 ? "Нет" : "Да")}" +
                                              $"\n\tСумма депозита: {usr.DepositAccount.GetBalance()}\n");
                        }
                        break;
                    case 8:
                        CurrentUser.DepositAccount.UnblockAccount(); break;
                    default:
                        Console.WriteLine("Завершение работы... ");
                        return;
                }
            }
        }

        private static List<User> LoadUsers()
        {
            return [new User("123123", "312321"), new User("ame", "duo"), new User("alex", "doe")];
        }

        public static User? GetUser(string name, string surname)
        {
            return Users.FirstOrDefault(usr => usr.Name == name && usr.Surname == surname);
        }

        public static void RegisterConsole()
        {
            Console.WriteLine("Зарегистрируйтесь: \n");
            Console.Write("Имя: ");
            var name = Console.ReadLine()!;
            Console.Write("Фамилия: ");
            var surname = Console.ReadLine()!;
            var registered = new User(name, surname);
            registered.NonDepositAccount.UnblockAccount();
            Users.Add(registered);
            CurrentUser = registered;
        }
    }
}
