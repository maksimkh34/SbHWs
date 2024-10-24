using System.Windows;
using HW16.Core;
using HW16.Core.Data;
using HW16.ViewModel;

namespace HW16;

public partial class RegisterUser
{
    public RegisterUser()
    {
        InitializeComponent();
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (await ((RegisterUserViewModel)DataContext).TryRegister()) Close();
    }
}