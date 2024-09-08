using System.Configuration;
using System.Data;
using System.Windows;
using HW10;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Database.SaveDatabase();
        }
    }

}
