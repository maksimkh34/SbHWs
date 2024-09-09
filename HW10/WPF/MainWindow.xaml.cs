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
        private MainViewModel viewModel
        {
            get => ((MainViewModel)DataContext);
            set => DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel = new MainViewModel();
            Database.ActiveEmployee?.SelectClient(viewModel.SelectedClient);
            viewModel.UpdateSelectedEmployee();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddNewClient();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var prevSelected = (Client)e.RemovedItems[0]!;
                var nextSelected = (Client)e.AddedItems[0]!;
                if (MainViewModel.CanChangeSelection(prevSelected, nextSelected)) return;
                    MessageBox.Show("Заполните все поля! ", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (sender is ListBox listBox) listBox.SelectedItem = prevSelected;
            }
            catch(IndexOutOfRangeException) { }
        }

        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortClientsByName();
        }

        private void SortByIdButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SortClientsById();
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateClientsView();
        }
    }
}