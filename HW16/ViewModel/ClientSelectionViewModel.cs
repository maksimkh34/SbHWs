using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HW16.Core;
using Database = HW16.Core.Data.Database;

namespace HW16.ViewModel;

public class ClientSelectionViewModel : BaseViewModel
{
    private ObservableCollection<Client> Clients { get; set; }
    public int SelectedClientIndex { get; set; }
    private List<Client> _selectedClients;
    public List<Client> SelectedClients
    {
        get => _selectedClients;
        set
        {
            _selectedClients = value;
            OnPropertyChanged();
        }
    }

    public async Task<bool?> CellEdited(DataGridCellEditEndingEventArgs e)
    {
        if (e.EditingElement is not TextBox textBox) return null;
        var newValue = textBox.Text;

        if (e.Row.Item is not Client editedClient) return null;
        if(e.Column.Header is not string columnName) return null;
        var oldValue = GetOldValue(editedClient, columnName);
                
        var propertyInfo = typeof(Client).GetProperty(ColumnNameToPropertyName(columnName));
        if (propertyInfo == null) return null;
        if (Database.IsValidValue(newValue, propertyInfo))
        {
            propertyInfo.SetValue(editedClient, Convert.ChangeType(newValue, propertyInfo.PropertyType));
            var updateResult = await Database.UpdateAsync(editedClient);
            if(!updateResult) MessageBox.Show("Ошибка обновления данных: " + updateResult.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            await RefreshClients();
            return true;
        }

        textBox.Text = oldValue;
        return false;
    }

    private static string ColumnNameToPropertyName(string columnName)
    {
        return (columnName switch
        {
            "ID" => nameof(Client.Id),
            "Фамилия" => nameof(Client.Surname),
            "Имя" => nameof(Client.Name),
            "Отчество" => nameof(Client.Patronymic),
            "Телефон" => nameof(Client.PhoneNumber),
            "Email" => nameof(Client.Email),
            _ => null
        })!;
    }
    
    private static string GetOldValue(Client client, string columnName)
    {
        return (columnName switch
        {
            "ID" => client.Id.ToString(),
            "Фамилия" => client.Surname,
            "Имя" => client.Name,
            "Отчество" => client.Patronymic,
            "Телефон" => client.PhoneNumber,
            "Email" => client.Email,
            _ => null
        })!;
    }

    public Client? GetSelectedClient()
    {
        return SelectedClients.Count != 1 ? null : SelectedClients.First();
    }

    public ICommand ProcessSelectedCommand { get; }

    private async void ProcessSelected()
    {
        switch (MessageBox.Show("Вы точно хотите удалить этих клиентов?", "Подтверждение", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No))
        {
            case MessageBoxResult.Yes:
            case MessageBoxResult.OK:
                foreach (var client in SelectedClients)
                {
                    var result = await Database.DeleteAsync(client);
                    if (result.Success) continue;
                    MessageBox.Show("Ошибка удаления клиентов: " + result.Message, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }

                var msg = SelectedClients.Count switch
                {
                    0 => "Ни одного клиента не было удалено.",
                    _ when SelectedClients.Count % 10 == 1 && SelectedClients.Count % 100 != 11 => $"Удален {SelectedClients.Count} клиент.",
                    _ when (SelectedClients.Count % 10 == 2 || SelectedClients.Count % 10 == 3 || SelectedClients.Count % 10 == 4) && !(SelectedClients.Count % 100 >= 11 && SelectedClients.Count % 100 <= 14) => $"Удалено {SelectedClients.Count} клиента.",
                    _ => $"Удалено {SelectedClients.Count} клиентов."
                };
                MessageBox.Show(msg, "ОК",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                SelectedClients.Clear();
                await RefreshClients();
                break;
            case MessageBoxResult.None:
            case MessageBoxResult.Cancel:
            case MessageBoxResult.No:
            default:
                return;
        }
    }

    private bool CanProcessSelected()
    {
        return SelectedClients.Count != 0;
    }
    
    public ListCollectionView ClientsView { get; } = null!;
    private string _idFilter = "";
    public string IdFilter
    {
        get => _idFilter;
        set
        {
            _idFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }

    private string _surnameFilter = "";
    public string SurnameFilter
    {
        get => _surnameFilter;
        set
        {
            _surnameFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }

    private string _nameFilter = "";
    public string NameFilter
    {
        get => _nameFilter;
        set
        {
            _nameFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }

    private string _patronymicFilter = "";
    public string PatronymicFilter
    {
        get => _patronymicFilter;
        set
        {
            _patronymicFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }

    private string _phoneNumberFilter = "";
    public string PhoneNumberFilter
    {
        get => _phoneNumberFilter;
        set
        {
            _phoneNumberFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }
    
    private string _emailFilter = "";
    public string EmailFilter
    {
        get => _emailFilter;
        set
        {
            _emailFilter = value;
            OnPropertyChanged();
            ClientsView.Refresh();
        }
    }

    public ClientSelectionViewModel()
    {
        _selectedClients = [];
        Database.CheckInitializeSync();
        var result = Database.Select<Client>();
        ProcessSelectedCommand = new RelayCommand(ProcessSelected, CanProcessSelected);

        if (!result.Success)
        {
            MessageBox.Show("Database loading failed: " + result.Message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Clients = [];
            return;
        }

        Clients = new ObservableCollection<Client>(result.Data);
        ClientsView = (CollectionViewSource.GetDefaultView(Clients) as ListCollectionView)!;
        ClientsView.Filter = ClientFilter;
    }

    public async Task RefreshClients()
    {
        var newClients = (await Database.SelectAsync<Client>()).Data;
        Clients.Clear();
        foreach (var client in newClients)
        {
            Clients.Add(client);
        }
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        ClientsView?.Refresh();
    }


    private bool ClientFilter(object item)
    {
        if (item is not Client client) return false;

        var matches = true;
        matches &= string.IsNullOrEmpty(IdFilter) || client.Id.ToString().Contains(IdFilter, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrEmpty(SurnameFilter) || client.Surname.Contains(SurnameFilter, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrEmpty(NameFilter) || client.Name.Contains(NameFilter, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrEmpty(PatronymicFilter) || client.Patronymic.Contains(PatronymicFilter, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrEmpty(PhoneNumberFilter) || client.PhoneNumber.Contains(PhoneNumberFilter, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrEmpty(EmailFilter) || client.Email.Contains(EmailFilter, StringComparison.OrdinalIgnoreCase);

        return matches;
    }
}

public class RelayCommand(Action processSelected, Func<bool> canProcessSelected) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canProcessSelected.Invoke();
    }

    public void Execute(object? parameter)
    {
        processSelected.Invoke();
    }
    public event EventHandler? CanExecuteChanged;
}