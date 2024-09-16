using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12
{
    internal class User<T> where T : Account, new()
    {
        private readonly T account;
        public string Name { get; set; }
        public string Surname { get; set; }

        public User(Action<string> message, string name, string surname, uint balance)
        {
            Surname = surname;
            Name = name;
            Message = message;
            T acc = new();
            acc.Deposit(balance);
            account = acc;
        }

        public User(Action<string> message, string name, string surname) : this(message, name, surname, 0) { }

        public Action<string> Message;

        public void BlockAccount() => account.BlockAccount();
        public void UnblockAccount() => account.UnblockAccount();

        public void Deposit(uint amount)
        {
            switch (account.Deposit(amount))
            {
                case OperationResult.Success:
                    Message("На счет зачислено " + amount);
                    break;
                case OperationResult.Rejected:
                    Message("Операция запрещена. ");
                    break;
            }
        }

        public void Take(uint amount)
        {
            switch (account.Take(amount))
            {
                case OperationResult.Success:
                    Message("Со счета списано: " + amount);
                    break;
                case OperationResult.Rejected:
                    Message("Операция запрещена. ");
                    break;
                case OperationResult.NotEnoughBalance:
                    Message("Недостаточно баланса. ");
                    break;
            }
        }

        public void Transfer(User<T> user, uint amount)
        {
            switch (account.TransferTo(user.account, amount))
            {
                case OperationResult.Success:
                    Message($"Пользвателю {user.Name} переведено {amount}. ");
                    break;
                case OperationResult.Rejected:
                    Message("Операция запрещена. ");
                    break;
                case OperationResult.NotEnoughBalance:
                    Message("Недостаточно баланса. ");
                    break;
            }
        }
    }
}
