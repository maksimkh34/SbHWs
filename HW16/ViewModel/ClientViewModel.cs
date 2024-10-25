using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using HW16.Core;
using HW16.Core.Data;

namespace HW16.ViewModel;

public class ClientViewModel : BaseViewModel
{
    private Client CurrentClient { get; set; }
    public ObservableCollection<ProductSaleEntry> Sales { get; set; } = [];
    public ObservableCollection<ProductSaleEntry> SelectedSales { get; set; }

    // только для конструктора XAML
    public ClientViewModel() : this(new Client()) { }

    public ClientViewModel(Client client)
    {
        CurrentClient = client;
        SelectedSales = [];
        DeleteSelectedCommand = new RelayCommand(DeleteSelected, CanDeleteSelected);
        RefreshSalesTable();
    }
    
    public ICommand DeleteSelectedCommand { get; }

    private async void DeleteSelected()
    {
        if (SelectedSales.Count == 0) return;
        switch (MessageBox.Show("Вы точно хотите удалить выделенные записи? ", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Cancel))
        {
            case MessageBoxResult.Yes:
            case MessageBoxResult.OK:
                foreach (var sale in SelectedSales)
                {
                    var result = await Database.DeleteAsync(sale);
                    if (result.Success) continue;
                    MessageBox.Show("Ошибка удаления записи: " + result.Message, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                MessageBox.Show("Удалено записей: " + SelectedSales.Count, "ОК",
                    MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                SelectedSales.Clear();
                RefreshSalesTable();
                break;
            case MessageBoxResult.None:
            case MessageBoxResult.Cancel:
            case MessageBoxResult.No:
            default:
                break;
        }
    }

    private bool CanDeleteSelected()
    {
        return SelectedSales.Count > 0;
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
        OnPropertyChanged(nameof(Sales));
    }
}