﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HW10;

namespace WPF
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private int _selectedClientIndex;
        public int SelectedClientIndex
        {
            get => _selectedClientIndex;
            set
            {
                if (_selectedClientIndex == value) return;
                _selectedClientIndex = value;
                Database.ActiveEmployee?.SelectClient(SelectedClient.Id);
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedClient));
                OnPropertyChanged(nameof(SelectedEmployee));
                OnPropertyChanged(nameof(WindowTitle));
                OnPropertyChanged(nameof(ChangesNumber));
            }
        }

        public ObservableCollection<Client> Clients => new(Database.Clients);
        public Client SelectedClient => Clients[SelectedClientIndex];

        public Employee SelectedEmployee => Database.ActiveEmployee!;
        public string ChangesNumber => "Всего изменений внесено: " + Database.Changes.Count;
        public string Status => "Статус: " + (Database.ActiveEmployee?.GetType() == typeof(Manager) ? "Менеджер" : "Консультант");
        public bool CanAddClients => Database.ActiveEmployee?.GetType() == typeof(Manager);
        public bool CanSwitchClient => Database.ActiveEmployee?.DataFilled() ?? true;
        public string WindowTitle => "Клиент " + SelectedClient.Id;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSelectedEmployee() => OnPropertyChanged(nameof(SelectedEmployee));

        public ICommand ChangeSelectionCommand { get; }


        public void AddNewClient()
        {
            Database.Clients.Add(new Client());
            SelectedClientIndex = Clients.Count - 1;
            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(SelectedEmployee));
        }
    }
}
