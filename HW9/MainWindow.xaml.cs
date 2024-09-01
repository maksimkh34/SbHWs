using System.Globalization;
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

namespace HW9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MyWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Apply_Task1Button_Click(object sender, RoutedEventArgs e)
        {
            Task1OutListBox.ItemsSource = HW5.Task1.SplitText(Task1InTextBox.Text);
        }

        private void Apply_Task2Button_Click(object sender, RoutedEventArgs e)
        {
            // Task2OutLabel.Content = HW5.Task2.Reverse(Task2InTextBox.Text);
        }
    }

    public class Task2Converter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return HW5.Task2.Reverse(value?.ToString());
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}