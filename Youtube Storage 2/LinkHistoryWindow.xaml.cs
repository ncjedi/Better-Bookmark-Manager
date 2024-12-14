using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Youtube_Storage_2
{
    /// <summary>
    /// Interaction logic for LinkHistoryWindow.xaml
    /// </summary>
    public partial class LinkHistoryWindow : Window
    {
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        public LinkHistoryWindow()
        {
            InitializeComponent();

            Link selected = new Link();
            string textboxText = "";

            if (parent.FolderMenuList.SelectedItem != null)
            {
                selected = parent.GetLinkBySelected((MainWindow.Transfer)parent.FolderMenuList.SelectedItem);
            }

            foreach(string link in selected.getHistory())
            {
                textboxText += link;
                textboxText += "\n\n";
            }

            Text.Text = textboxText;
        }
    }
}
