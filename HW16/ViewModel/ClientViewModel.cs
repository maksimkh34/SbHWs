using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        RefreshSalesTable().GetAwaiter().GetResult();
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
                RefreshSalesTable().GetAwaiter().GetResult();
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

    public async Task RefreshSalesTable()
    {
        var result = await Database.SelectAsync<ProductSaleEntry>(entry => entry.Email == CurrentClient.Email);
        if (result.ErrorCode == Database.SelectErrorCode.RecordNotFound)
        {
            MessageBox.Show("Нет зарегистрированных продаж. ", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            Sales = [];
            return;
        }
        if (!result.Success)
        {
            MessageBox.Show("Ошибка загрузки базы данных: " + result.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            Sales = [];
        }
        Sales = new ObservableCollection<ProductSaleEntry>(result.Data);
        OnPropertyChanged(nameof(Sales));
    }

    public async Task<bool?> CellEdited(DataGridCellEditEndingEventArgs e)
    {
        if (e.EditingElement is not TextBox textBox) return null;
        var newValue = textBox.Text;

        if (e.Row.Item is not ProductSaleEntry editedEntry) return null;
        if(e.Column.Header is not string columnName) return null;
        var oldValue = GetOldValue(editedEntry, columnName);
                
        var propertyInfo = typeof(ProductSaleEntry).GetProperty(ColumnNameToPropertyName(columnName));
        if (propertyInfo == null) return null;
        if (Database.IsValidValue(newValue, propertyInfo))
        {
            propertyInfo.SetValue(editedEntry, Convert.ChangeType(newValue, propertyInfo.PropertyType));
            await Database.UpdateAsync(editedEntry);
            await RefreshSalesTable();
            return true;
        }

        textBox.Text = oldValue;
        return false;
    }
    
    private static string ColumnNameToPropertyName(string columnName)
    {
        return (columnName switch
        {
            "ID записи" => nameof(ProductSaleEntry.Id),
            "Email" => nameof(ProductSaleEntry.Email),
            "ID Продукта" => nameof(ProductSaleEntry.ProductId),
            "Наименование продукта" => nameof(ProductSaleEntry.ProductName),
            _ => null
        })!;
    }
    
    private static string GetOldValue(ProductSaleEntry entry, string columnName)
    {
        return (columnName switch
        {
            "ID записи" => entry.Id.ToString(),
            "Email" => entry.Email,
            "ID Продукта" => entry.ProductId.ToString(),
            "Наименование продукта" => entry.ProductName,
            _ => null
        })!;
    }

    public async Task OpenRegDialog()
    {
        new AddSaleDialog().ShowDialog();
        await RefreshSalesTable();
    }
}