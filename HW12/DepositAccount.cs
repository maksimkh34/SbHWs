using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12
{
    internal class DepositAccount : Account
    {
        private bool _canWithdraw = true;
        public float Interest { get; set; }

        public override OperationResult Take(uint amount)
        {        
            if (_canWithdraw) 
                return base.Take((uint)(amount * Interest));
            else 
                return OperationResult.Rejected;
        }

        public void ProhibitWithdrawal() => _canWithdraw = false;
        public void AllowWithdrawal() => _canWithdraw = true;
    }
}
