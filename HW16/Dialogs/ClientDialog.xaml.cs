using System.Windows;
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
}