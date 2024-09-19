namespace HW12
{
    public class UserAccount<T> where T : Account, new()
    {
        private readonly T _account;
        public bool IsBlocked() => _account.GetBlocked();
        public uint GetBalance() => _account.GetBalance();

        public UserAccount(Action<string> message, uint balance)
        {
            Message = message;
            T acc = new();
            acc.Deposit(balance);
            _account = acc;
        }
            
        public UserAccount(Action<string> message) : this(message, 0) { }

        public Action<string> Message;

        public void BlockAccount() { _account.BlockAccount(); Message.Invoke("Операции запрещены! ");}
        public void UnblockAccount() { _account.UnblockAccount(); Message.Invoke("Операции разрешены! "); }

        public void Deposit(uint amount)
        {
            switch (_account.Deposit(amount).Result)
            {
                case OperationResultEnum.Success:
                    Message("На счет зачислено " + amount);
                    break;
                case OperationResultEnum.Rejected:
                    Message("Операция запрещена. ");
                    break;
                case OperationResultEnum.NotEnoughBalance:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Take(uint amount)
        {
            switch (_account.Take(amount).Result)
            {
                case OperationResultEnum.Success:
                    Message("Со счета списано: " + amount);
                    break;
                case OperationResultEnum.Rejected:
                    Message("Операция запрещена. ");
                    break;
                case OperationResultEnum.NotEnoughBalance:
                    Message("Недостаточно баланса. ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Transfer(User user, uint amount)
        {
            switch (_account.TransferTo(user.NonDepositAccount._account, amount).Result)
            {
                case OperationResultEnum.Success:
                    Message($"Пользователю {user.Name} переведено {amount}. ");
                    break;
                case OperationResultEnum.Rejected:
                    Message("Операция запрещена. ");
                    break;
                case OperationResultEnum.NotEnoughBalance:
                    Message("Недостаточно баланса. ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
