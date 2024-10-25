using System.Windows;
using HW16.ViewModel;

namespace HW16;

public partial class AddSaleDialog : Window
{
    private AddSaleViewModel ViewModel
    {
        get => (AddSaleViewModel)DataContext;
        set => DataContext = value;
    }
    
    public AddSaleDialog()
    {
        InitializeComponent();
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var close = await ViewModel.TryRegister();
        if(close) Close();
    }
}