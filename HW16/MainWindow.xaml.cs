using System.Data.Common;
using System.Data.SqlClient;
using System.Windows;
using System.Data.OleDb;
using System.Windows.Controls;

namespace HW16;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        await Database.Initialize();
        Database.Select<ProductSaleEntry>();
    }
    
}
