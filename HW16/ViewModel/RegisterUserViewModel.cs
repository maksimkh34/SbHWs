using System.Windows;
using HW16.Core;
using HW16.Core.Data;

namespace HW16.ViewModel;

public class RegisterUserViewModel : BaseViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public async Task<bool> TryRegister()
    {
        if (!Database.IsValidValue(PhoneNumber, typeof(Client).GetProperty(nameof(Client.PhoneNumber))!))
        {
            MessageBox.Show("Invalid phone number. Format:\n+[number]\nProvided:\n" + PhoneNumber,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }
        if (!Database.IsValidValue(Email, typeof(Client).GetProperty(nameof(Client.Email))!))
        {
            MessageBox.Show("Invalid email. Format:\n[email]@[domain].[suffix]\nProvided:\n" + Email,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }

        if (!Database.IsValidValue(Name, typeof(Client).GetProperty(nameof(Client.Name))!) || 
            !Database.IsValidValue(Surname, typeof(Client).GetProperty(nameof(Client.Surname))!))
        {
            MessageBox.Show("Invalid Name / Surname.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }

        var result = await Database.InsertAsync(new Client
        {
            Email = Email,
            Name = Name,
            Patronymic = Patronymic,
            PhoneNumber = PhoneNumber,
            Surname = Surname
        });

        if (!result.Success)
        {
            MessageBox.Show("Error inserting new client to database: " + result.Message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        
        MessageBox.Show("Клиент зарегистрирован! ",
            "OK",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        return true;
    }
}