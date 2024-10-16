using System.Data.SqlClient;
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

namespace HW16;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        string connectionString = "Data Source=(localdb)\\SkillBoxDBLecture;Initial Catalog=SkillBoxDBLecture;Integrated Security=True;Pooling=False;Encrypt=False";
        var connection = new SqlConnection(connectionString);
        connection.Open();
    }
}