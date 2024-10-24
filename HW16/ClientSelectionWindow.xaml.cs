using System.Windows;
using HW16.ViewModel;
using Database = HW16.Core.Data.Database;

namespace HW16;

public partial class ClientSelectionWindow
{
    public ClientSelectionWindow()
    {
        InitializeComponent();
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await Database.Initialize();
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        new RegisterUser().ShowDialog();
        await ((ClientSelectionViewModel)DataContext).RefreshClients();
    }
}
