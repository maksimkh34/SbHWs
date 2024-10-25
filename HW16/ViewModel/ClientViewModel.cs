using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HW16.Core;
using HW16.Core.Data;

namespace HW16.ViewModel;

public class ClientViewModel : BaseViewModel
{
    public Client CurrentClient { get; set; }
    public ObservableCollection<ProductSaleEntry> Sales { get; set; }

    // только для конструктора XAML
    public ClientViewModel()
    {
        CurrentClient = new Client();
        Sales = [];
    }

    public ClientViewModel(Client client)
    {
        CurrentClient = client;
        ProcessSelectedCommand = new RelayCommand(ProcessSelected, CanProcessSelected);
        RefreshSalesTable();
    }
    
    public ICommand ProcessSelectedCommand { get; }

    private async void ProcessSelected()
    {
        
    }

    private bool CanProcessSelected()
    {
        return true;
    }

    public void RefreshSalesTable()
    {
        var result = Database.Select<ProductSaleEntry>(entry => entry.Email == CurrentClient.Email);
        if (result.ErrorCode == Database.SelectErrorCode.RecordNotFound)
        {
            MessageBox.Show("Нет зарегистрированных продаж. ", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            Sales = [];
            return;
        }
        if (!result.Success)
        {
            MessageBox.Show("Ошибка загрузки базы данных. ", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            Sales = [];
        }
        Sales = new ObservableCollection<ProductSaleEntry>(result.Data);
    }
}