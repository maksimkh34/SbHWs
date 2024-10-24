using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace HW16;

public class ClientSelectionViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Client> Clients { get; }
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
        var task = Task.Run(() => Database.SelectAsync<Client>());
        var result = task.GetAwaiter().GetResult();

        if (!result.Success)
        {
            MessageBox.Show("Database loading failed: " + task.Result.Message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Clients = [];
            return;
        }

        Clients = new ObservableCollection<Client>(task.Result.Data);
        ClientsView = (CollectionViewSource.GetDefaultView(Clients) as ListCollectionView)!;
        ClientsView.Filter = ClientFilter;
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



    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}