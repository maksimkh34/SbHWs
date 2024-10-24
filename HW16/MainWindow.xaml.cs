using System.Windows;

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
        var selectResult = await Database.SelectAsync<Client>(client => client.Name == "name" || client.Name == "name2");
        if (selectResult.Success)
        {
            foreach (var client in selectResult.Data)
            {
                MessageBox.Show($"Client: {client.Id}, {client.Name} {client.Surname}, Email: {client.Email}");
            }
        }
        else
        {
            MessageBox.Show($"Select failed: {selectResult.Message}, ErrorCode: {selectResult.ErrorCode}");
        }
        /*
        var insertResult = await Database.InsertAsync(new Client
        {
            Surname = "Doe",
            Name = "John",
            Patronymic = "Middle",
            PhoneNumber = "375123456789",
            Email = "john.doe@example.com"
        });

        MessageBox.Show(insertResult ? "Insert successful." : $"Insert failed: {insertResult.Message}");
        
        var updateResult = await Database.UpdateAsync(new Client
        {
            Id = 1,
            Surname = "Doe",
            Name = "Jane",
            Patronymic = "Middle",
            PhoneNumber = "375987654321",
            Email = "jane.doe@example.com"
        });

        MessageBox.Show(updateResult
            ? "Update successful."
            : $"Update failed: {updateResult.Message}, ErrorCode: {updateResult.ErrorCode}");
        
        var deleteResult = await Database.DeleteAsync(new Client
        {
            Id = 1,
            Surname = "Doe",
            Name = "Jane",
            Patronymic = "Middle",
            PhoneNumber = "375987654321",
            Email = "jane.doe@example.com"
        });

        MessageBox.Show(deleteResult
            ? "Delete successful."
            : $"Delete failed: {deleteResult.Message}, ErrorCode: {deleteResult.ErrorCode}");
            */
    }
    
}
