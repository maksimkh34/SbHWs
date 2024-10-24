using System.Windows;
using HW16.Data;

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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        new RegisterUser().ShowDialog();
        ((ClientSelectionViewModel)DataContext).RefreshClients();
    }
}
