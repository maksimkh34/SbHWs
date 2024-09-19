
namespace HW12
{
    public class DepositAccount : Account
    {
        public float Interest { get; set; } = 1.2f;

        public override OperationResult Deposit(uint amount)
        {
            return base.Deposit((uint)(amount * Interest));
        }
    }
}
