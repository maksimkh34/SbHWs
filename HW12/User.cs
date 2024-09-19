namespace HW12
{
    public class User(string name, string surname, Action<string>? message = null)
    {
        public string Name { get; set; } = name;
        public string Surname { get; set; } = surname;

        public Action<string>? MessageAction = message;

        public UserAccount<DepositAccount> DepositAccount { get; } = new(message ?? Console.WriteLine);
        public UserAccount<NonDepositAccount> NonDepositAccount { get; } = new(message ?? Console.WriteLine);

        public void SetMsg(Action<string> action)
        {
            DepositAccount.Message = action;
            NonDepositAccount.Message = action;
            MessageAction = action;
        }

        public void Message(string msg)
        {
            MessageAction?.Invoke(msg);
        }
    }
}