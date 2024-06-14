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

namespace Youtube_Storage_2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ImportButtonPressed(object sender, RoutedEventArgs e)
        {
            parent.ImportPressed();
        }
    }
}
