using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HW12;
using HW12UI.ViewModel;

namespace HW12UI.View
{
    /// <summary>
    /// Логика взаимодействия для ClientPanel.xaml
    /// </summary>
    public partial class ClientPanel : Window
    {
        private readonly User _selectedUser;

        private ClientPanelViewModel ViewModel
        {
            get => (ClientPanelViewModel)DataContext;
            set => DataContext = value;
        }

        public ClientPanel(User user)
        {
            InitializeComponent();
            _selectedUser = user;
        }

        private void ClientPanel_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new ClientPanelViewModel(_selectedUser, () => { SumTextBox.Text = "0"; 
                SumTextBox.Focus(); SumTextBox.SelectAll();
            });
        }
    }
}
