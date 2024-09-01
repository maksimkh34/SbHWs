using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HW9
{
    public class MyWindow : Window
    {
        public string GetSelf() => this.ToString() ?? string.Empty;
    }
}
