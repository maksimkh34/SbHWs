namespace HW12
{
    internal abstract class Account
    {
        private uint balance = 0;
        private bool blocked = false;

        public Account(uint balance)
        {
             this.balance = balance; 
        }

        public Account() : this(0) { }

        public uint GetBalance() => balance;
        public void BlockAccount() => blocked = true;
        public void UnblockAccount() => blocked = false;

        public OperationResult Deposit(uint amount)
        {
            if (blocked) return OperationResult.Rejected;
            balance += amount;
            return OperationResult.Success;
        }

        public virtual OperationResult Take(uint amount) 
        {
            if (blocked) return OperationResult.Rejected;
            if (balance - amount < 0) return OperationResult.NotEnoughBalance;
            balance -= amount;
            return OperationResult.Success;
        }

        public OperationResult TransferTo(Account account, uint amount) 
        { 
            if(blocked) return OperationResult.Rejected;
            if(Take(amount) == OperationResult.NotEnoughBalance) return OperationResult.NotEnoughBalance;
            if(account.Deposit(amount) == OperationResult.Rejected) return OperationResult.Rejected;
            return OperationResult.Success;
        }
    }

    public enum OperationResult
    {
        Success,
        NotEnoughBalance,
        Rejected
    }
}
