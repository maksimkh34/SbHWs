namespace HW12
{
    internal class User(string name, string surname)
    {
        public string Name { get; set; } = name;
        public string Surname { get; set; } = surname;

        public UserAccount<DepositAccount> DepositAccount { get; } = new(Console.WriteLine);
        public UserAccount<NonDepositAccount> NonDepositAccount { get; } = new(Console.WriteLine);
    }
}