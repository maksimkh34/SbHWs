using System.Windows;
using System.Windows.Navigation;
using System.Data.SqlClient;
using HW16.Core;
using HW16.Core.Data;
using HW16.ViewModel;

namespace HW16;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        await Database.Initialize();
        
        var sales = await Database.SelectAsync<ProductSaleEntry>();
        var sale = sales.Data.First();
        sale.Email = "changedEmail@ma.il";
        await Database.UpdateAsync(sale);
    }
}