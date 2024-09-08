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

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var role = (string)((ComboBoxItem)EmployeeComboBox.SelectedItem).Content;
            HW10.Database.ActiveEmployee = role switch
            {
                "Менеджер" => new HW10.Manager(),
                "Консультант" => new HW10.Consultant(),
                _ => HW10.Database.ActiveEmployee
            };
            new MainWindow().Show();
            Close();
        }
    }
}
