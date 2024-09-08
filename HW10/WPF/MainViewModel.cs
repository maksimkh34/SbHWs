﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
                OnPropertyChanged(nameof(ChangesNumberTextBlockTitle));
            }
        }

        public List<Client> Clients => Database.Clients;
        public Client SelectedClient => Clients[SelectedClientIndex];

        public Employee SelectedEmployee => Database.ActiveEmployee!;
        public string ChangesNumberTextBlockTitle => "Всего изменений внесено: " + Database.Changes.Count;
        public string WindowTitle => "Клиент " + SelectedClient.Id;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSelectedEmployee() => OnPropertyChanged(nameof(SelectedEmployee));
    }
}