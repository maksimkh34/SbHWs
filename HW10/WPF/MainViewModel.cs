using System;
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

        public ObservableCollection<Client> Clients
        {
            get => new(Database.Clients);
            set => Database.Clients = value.ToList();
        }

        public Client SelectedClient => _selectedClientIndex == -1 ? Clients.First() : Clients[SelectedClientIndex];

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

        public void AddNewClient()
        {
            Database.Clients.Add(new Client());
            SelectedClientIndex = Clients.Count - 1;
            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(SelectedEmployee));
        }

        public static bool CanChangeSelection(Client prev, Client next) => prev.DataFilled() || !next.DataFilled();

        public void SortClientsByName()
        {
            SortClientsByProp("Name");
        }

        public void SortClientsById()
        {
            SortClientsByProp("Id");
        }

        private void SortClientsByProp(string propertyName)
        {
            var sortedAsc = Clients.OrderBy(x => x.GetType().GetProperty(propertyName)?.GetValue(x, null)).ToList();
            Clients = Database.ClientDatabasesEquals(Clients, sortedAsc)
                ? new ObservableCollection<Client>(Clients.OrderByDescending(x => x.GetType().GetProperty(propertyName)?.GetValue(x, null)))
                : new ObservableCollection<Client>(sortedAsc);
            OnPropertyChanged(nameof(Clients));
        }

        public void UpdateClientsView() => OnPropertyChanged(nameof(Clients));

        public void RemoveSelected()
        {
            Database.Clients.Remove(SelectedClient);
            OnPropertyChanged(nameof(Clients));
        }
    }
}
