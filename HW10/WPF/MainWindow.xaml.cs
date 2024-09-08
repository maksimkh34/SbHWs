using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HW10;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object? _previousSelectedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
            Database.ActiveEmployee?.SelectClient(((MainViewModel)DataContext).SelectedClient);
            ((MainViewModel)DataContext).UpdateSelectedEmployee();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).AddNewClient();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var prevSelected = (Client)e.RemovedItems[0]!;
                var nextSelected = (Client)e.AddedItems[0]!;
                if (prevSelected.Name != "" &&
                    prevSelected.Surname != "" &&
                    prevSelected.Passport != "" &&
                    prevSelected.PhoneNumber != "") return;
                if (nextSelected.Name == "" ||
                    nextSelected.Surname == "" ||
                    nextSelected.Passport == "" ||
                    nextSelected.PhoneNumber == "") return;
                    MessageBox.Show("Заполните все поля! ", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (sender is ListBox listBox) listBox.SelectedItem = prevSelected;
            }
            catch(IndexOutOfRangeException) { }
        }
    }
}