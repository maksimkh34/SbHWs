using System.Windows;
using HW12UI.ViewModel;

namespace HW12UI.View
{
    public partial class CreateNewUserDialog : Window
    {
        private CreateNewUserViewModel ViewModel
        {
            get => (CreateNewUserViewModel)DataContext;
            set => DataContext = value;
        }

        public CreateNewUserDialog()
        {
            InitializeComponent();
        }

        private void CreateNewUserDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new CreateNewUserViewModel(ShowMsg);
        }

        public void ShowMsg(string msg) =>
            MessageBox.Show(msg, "Внимание! ", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}
