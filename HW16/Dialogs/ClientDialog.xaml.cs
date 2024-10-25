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

    private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.RefreshSalesTable();
    }

    private void SalesDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.SelectedSales = new ObservableCollection<ProductSaleEntry>(
            SalesDataGrid.SelectedItems.Cast<ProductSaleEntry>());
    }
}