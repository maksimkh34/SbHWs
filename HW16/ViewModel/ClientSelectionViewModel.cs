using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using HW16.Core;
using Database = HW16.Core.Data.Database;

namespace HW16.ViewModel;

public class ClientSelectionViewModel : BaseViewModel
{
    private ObservableCollection<Client> Clients { get; set; }
    public ListCollectionView  ClientsView { get; } = null!;
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
        Database.CheckInitializeSync();
        var result = Database.Select<Client>();

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
        ClientsView.Refresh();
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