using HW12;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HW12UI.Core;

namespace HW12UI.ViewModel
{
    internal class ClientPanelViewModel : INotifyPropertyChanged
    {
        public User SelectedUser { get; set; }
        public string OperationSumStr { get; set; } = "0";
        public string ClientNameMsg => $"Клиент {SelectedUser.Surname} {SelectedUser.Name}";
        public ICommand DepositCommand { get; set; }
        public ICommand TakeCommand { get; set; }
        public ICommand UnblockNonDepositCommand { get; set; }
        public ICommand BlockNonDepositCommand { get; set; }

        public string NonDepositAccountBlockedTitle =>
            "Статус: " + (SelectedUser.NonDepositAccount.IsBlocked() ? "Заблокирован" : "Разблокирован");
        public string NonDepositAccountBalance =>
            "Баланс: " + SelectedUser.NonDepositAccount.GetBalance();



        public ClientPanelViewModel(User selectedUser, Action clearSumTextBoxAction)
        {
            SelectedUser = selectedUser;
            BlockNonDepositCommand = new DelegateCommand(BlockNonDeposit);
            UnblockNonDepositCommand = new DelegateCommand(UnblockNonDeposit);
            DepositCommand = new DelegateCommand(Deposit, commandExecuted: clearSumTextBoxAction);
            TakeCommand = new DelegateCommand(Take, commandExecuted: clearSumTextBoxAction);
        }
        // нужен только для удобства работы с вьюмоделью в конструкторе xaml
        public ClientPanelViewModel() : this(new User("-", "-"), () => { }) { }

        public void Deposit(object obj)
        {
            if (!uint.TryParse(OperationSumStr, out var sum))
            {
                SelectedUser.Message("Введена неправильная сумма! "); return;
            }
            SelectedUser.NonDepositAccount.Deposit(sum);
            OnPropertyChanged(nameof(NonDepositAccountBalance));
        }

        public void Take(object obj)
        {
            if (!uint.TryParse(OperationSumStr, out var sum))
            {
                SelectedUser.Message("Введена неправильная сумма! "); return;
            }
            SelectedUser.NonDepositAccount.Take(sum);
            OnPropertyChanged(nameof(NonDepositAccountBalance));
        }

        public void UnblockNonDeposit(object obj)
        {
            SelectedUser.NonDepositAccount.UnblockAccount();
            OnPropertyChanged(nameof(NonDepositAccountBlockedTitle));
        }

        public void BlockNonDeposit(object obj)
        {
            SelectedUser.NonDepositAccount.BlockAccount();
            OnPropertyChanged(nameof(NonDepositAccountBlockedTitle));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
