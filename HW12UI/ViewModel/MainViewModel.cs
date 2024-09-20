using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HW12;
using HW12UI.Core;
using HW12UI.View;

namespace HW12UI.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private User? _selectedClient;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> Users => Program.Users;

        public MainViewModel()
        {
            foreach (var user in Users)
            {
                user.SetMsg(Message);
            }
            SelectClientCommand = new DelegateCommand(SelectClient);
        }

        public void Message(string msg)
        {
            MessageBox.Show(msg, "Внимание! ", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public User? SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsClientWarningDisplayable));
                OnPropertyChanged(nameof(IsClientWarningDisplayableBtn));
            }
        }

        public ICommand SelectClientCommand { get; }
        public Visibility IsClientWarningDisplayable => SelectedClient == null ? Visibility.Visible : Visibility.Collapsed;
        public bool IsClientWarningDisplayableBtn => SelectedClient != null;

        private static void SelectClient(object obj)
        {
            if (obj is not User user) return;
            new ClientPanel(user).ShowDialog();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
