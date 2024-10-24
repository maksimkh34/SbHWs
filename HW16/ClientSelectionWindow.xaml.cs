using System.Windows;
using System.Windows.Controls;
using HW16.Core;
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

    private async void RegisterButton_OnClick(object sender, RoutedEventArgs e)
    {
        new RegisterUser().ShowDialog();
        await ((ClientSelectionViewModel)DataContext).RefreshClients();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ((ClientSelectionViewModel)DataContext).SelectedClients = ClientsDataGrid.SelectedItems.Cast<Client>().ToList();
    }

    private void SelectClient_OnClick(object sender, RoutedEventArgs e)
    {
        var client = ((ClientSelectionViewModel)DataContext).GetSelectedClient();
        if (client == null)
        {
            MessageBox.Show("Выберите ровно одного клиента. ", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            return;
        }

        var vm = new ClientViewModel { CurrentClient = client };
        new ClientDialog{DataContext = vm}.ShowDialog();
    }
}
