using HW12UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HW12UI.View
{
    public partial class JournalWindow
    {
        private JournalViewModel ViewModel
        {
            get => (JournalViewModel)DataContext;
            set => DataContext = value;
        }

        public JournalWindow()
        {
            InitializeComponent();
        }

        private void JournalWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new JournalViewModel();
        }
    }
}
