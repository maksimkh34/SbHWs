using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace HW12UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

    }

    public static class Extension {
        public static void Show(this string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
