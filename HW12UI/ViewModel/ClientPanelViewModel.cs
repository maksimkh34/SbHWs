using HW12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HW12UI.Core;

namespace HW12UI.ViewModel
{
    internal class ClientPanelViewModel(User selectedUser)
    {
        public User SelectedUser { get; set; } = selectedUser;
        public string OperationSumStr { get; set; } = "0";
        public string ClientNameMsg => $"Клиент {SelectedUser.Surname} {SelectedUser.Name}";
        public ICommand DepositCommand { get; } = new DelegateCommand(Deposit);
        public ICommand UnblockNonDepositCommand { get; } = new DelegateCommand(Unblock);


        // нужен только для удобства работы с вьюмоделью в конструкторе xaml
        public ClientPanelViewModel() : this(new User("-", "-")) { }

        public static void Deposit(object obj)
        {
            if (obj is not IEnumerable<object> items) return;
            var objects = items as object[] ?? items.ToArray();
            var operationSum = (string)objects.ElementAt(0);
            var user = (User)objects.ElementAt(1);
            if(!uint.TryParse(operationSum, out var sum)) return;
            user.NonDepositAccount.Deposit(sum);
        }

        public static void Unblock(object obj)
        {
            if (obj is not User user) return;
            user.NonDepositAccount.UnblockAccount();
        }
    }
}
