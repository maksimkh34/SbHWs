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
        public string DepositOperationSumStr { get; set; } = "0";
        public string ClientNameMsg => $"Клиент {SelectedUser.Surname} {SelectedUser.Name}";

        public ICommand DepositCommand { get; set; }
        public ICommand TakeCommand { get; set; }
        public ICommand UnblockNonDepositCommand { get; set; }
        public ICommand BlockNonDepositCommand { get; set; }
        public ICommand OpenDepositCommand { get; }
        public ICommand WithdrawDepositCommand { get; }
        public ICommand AllowWithdrawalCommand { get; }

        public bool CanOpenDeposit => SelectedUser.DepositAccount.IsBlocked() && SelectedUser.DepositAccount.GetBalance() == 0;
        public bool CanWithdrawDeposit => !SelectedUser.DepositAccount.IsBlocked() && SelectedUser.DepositAccount.GetBalance() != 0;
        public bool CanAllowWithdrawal => SelectedUser.DepositAccount.IsBlocked() && SelectedUser.DepositAccount.GetBalance() != 0;

        public string NonDepositAccountBlockedTitle =>
            "Статус: " + (SelectedUser.NonDepositAccount.IsBlocked() ? "Заблокирован" : "Разблокирован");
        public string NonDepositAccountBalance =>
            "Баланс: " + SelectedUser.NonDepositAccount.GetBalance();
        public string DepositAccountBalance =>
            "Баланс: " + SelectedUser.DepositAccount.GetBalance();

        public ClientPanelViewModel(User selectedUser, Action clearSumTextBoxAction)
        {
            SelectedUser = selectedUser;
            BlockNonDepositCommand = new DelegateCommand(BlockNonDeposit);
            UnblockNonDepositCommand = new DelegateCommand(UnblockNonDeposit);
            DepositCommand = new DelegateCommand(Deposit, commandExecuted: clearSumTextBoxAction);
            TakeCommand = new DelegateCommand(Take, commandExecuted: clearSumTextBoxAction);
            OpenDepositCommand = new DelegateCommand(OpenDeposit, commandExecuted: clearSumTextBoxAction);
            WithdrawDepositCommand = new DelegateCommand(WithdrawDeposit, commandExecuted: clearSumTextBoxAction);
            AllowWithdrawalCommand = new DelegateCommand(AllowDepositWithdrawal, commandExecuted: clearSumTextBoxAction);
        }
        // нужен только для удобства работы с вьюмоделью в конструкторе xaml
        public ClientPanelViewModel() : this(new User("-", "-"), () => { }) { }

        public void OpenDeposit(object obj)
        {
            if(!uint.TryParse(DepositOperationSumStr, out var sum)) {SelectedUser.Message("Введена неправильная сумма! "); return;}
            SelectedUser.DepositAccount.UnblockAccount(true);
            SelectedUser.DepositAccount.Deposit(sum);
            SelectedUser.DepositAccount.BlockAccount(true);
            OnPropertyChanged(nameof(DepositAccountBalance));
            OnPropertyChanged(nameof(CanOpenDeposit));
            OnPropertyChanged(nameof(CanWithdrawDeposit));
            OnPropertyChanged(nameof(CanAllowWithdrawal));
        }

        public void AllowDepositWithdrawal(object obj)
        {
            SelectedUser.DepositAccount.UnblockAccount();
            OnPropertyChanged(nameof(CanWithdrawDeposit));
            OnPropertyChanged(nameof(CanOpenDeposit));
        }

        public void WithdrawDeposit(object obj)
        {
            if (!uint.TryParse(DepositOperationSumStr, out var sum))
            {
                SelectedUser.Message("Введена неправильная сумма! "); return;
            }
            SelectedUser.DepositAccount.Take(sum);
            OnPropertyChanged(nameof(DepositAccountBalance));
        }

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

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, newValue)) return false;
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;

        }
    }
}
