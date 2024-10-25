using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using HW16.Core;
using HW16.ViewModel;

namespace HW16;

public partial class ClientDialog
{
    private ClientViewModel ViewModel
    {
        get => (ClientViewModel)DataContext;
        set => DataContext = value;
    }
    
    
    public ClientDialog()
    {
        InitializeComponent();
    }

    private async void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        await ViewModel.RefreshSalesTable();
    }

    private void SalesDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SelectedSales = new ObservableCollection<ProductSaleEntry>(
            SalesDataGrid.SelectedItems.Cast<ProductSaleEntry>());
    }


    private async void SalesDataGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        var result = await ViewModel.CellEdited(e);
        switch (result)
        {
            case null:
                return;
            case false:
                MessageBox.Show("Ошибка изменения данных", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                break;
        }
    }

    private async void RegisterEntry_OnClick(object sender, RoutedEventArgs e)
    {
        await ViewModel.OpenRegDialog();
    }
}