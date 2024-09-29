namespace HW12
{
    public class UserAccount<T> where T : Account, new()
    {
        private readonly T _account;
        public bool IsBlocked() => _account.GetBlocked();
        public uint GetBalance() => _account.GetBalance();
        public string Username;

        public UserAccount(Action<string> message, uint balance, string username)
        {
            Message = message;
            Username = username;
            T acc = new();
            acc.Deposit(balance);
            _account = acc;
        }
            
        public UserAccount(Action<string> message, string username) : this(message, 0, username) { }

        public Action<string> Message;

        public void BlockAccount(bool silent = false) 
        { _account.BlockAccount(); 
            Journal.Register(new AccountClosedArgs(typeof(T) == typeof(DepositAccount), Username));
            if (!silent) Message.Invoke("Операции запрещены! ");}

        public void UnblockAccount(bool silent = false)
        {
            _account.UnblockAccount();
            Journal.Register(new AccountOpenedArgs(typeof(T) == typeof(DepositAccount), Username));
            if (!silent) Message.Invoke("Операции разрешены! ");
        }

        public void Deposit(uint amount)
        {
            switch (_account.Deposit(amount).Result)
            {
                case OperationResultEnum.Success:
                    Message("На счет зачислено " + amount);
                    Journal.Register(new DepositedArgs(amount, Username));
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
            try
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Account.NotEnoughBalanceException)
            {
                Message("Недостаточно баланса. ");
            }
        }

        public void Transfer(User user, uint amount)
        {
            try
            {
                switch (_account.TransferTo(user.NonDepositAccount._account, amount).Result)
                {
                    case OperationResultEnum.Success:
                        Message($"Пользователю {user.Name} переведено {amount}. ");
                        Journal.Register(new TransferredArgs(amount, Username, user.Surname + " " + user.Name));
                        break;
                    case OperationResultEnum.Rejected:
                        Message("Операция запрещена. ");
                        break;
                    case OperationResultEnum.NotEnoughBalance:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Account.NotEnoughBalanceException)
            {
                Message("Недостаточно баланса. ");
            }
        }
    }
}
