namespace HW12
{
    public abstract class Account(uint balance)
    {
        private bool _blocked = true;

        protected Account() : this(0) { }

        public uint GetBalance() => balance;
        public bool GetBlocked() => _blocked;
        public void BlockAccount() => _blocked = true;
        public void UnblockAccount() => _blocked = false;

        public virtual OperationResult Deposit(uint amount)
        {
            if (_blocked) return new OperationResult(OperationResultEnum.Rejected, 0);
            balance += amount;
            return new OperationResult(OperationResultEnum.Success, amount);
        }

        public virtual OperationResult Take(uint amount) 
        {
            if (_blocked) return new OperationResult(OperationResultEnum.Rejected, 0);
            if ((int)balance - amount < 0) return new OperationResult(OperationResultEnum.NotEnoughBalance, 0);
            balance -= amount;
            return new OperationResult(OperationResultEnum.Success, amount);
        }

        public virtual OperationResult TransferTo(Account account, uint amount) 
        { 
            if(_blocked) return new OperationResult(OperationResultEnum.Rejected, 0);
            if(Take(amount) == new OperationResult(OperationResultEnum.NotEnoughBalance, 0)) return new OperationResult(OperationResultEnum.NotEnoughBalance, 0);
            return account.Deposit(amount) == new OperationResult(OperationResultEnum.Rejected, 0) ? 
                new OperationResult(OperationResultEnum.Rejected, 0) : 
                new OperationResult(OperationResultEnum.Success, amount);
        }
    }

    public record OperationResult(OperationResultEnum R, uint V)
    {
        public OperationResultEnum Result { get; set; } = R;
        public uint OperationValue { get; set; } = V;
    }

    public enum OperationResultEnum
    {
        Success,
        NotEnoughBalance,
        Rejected
    }
}
