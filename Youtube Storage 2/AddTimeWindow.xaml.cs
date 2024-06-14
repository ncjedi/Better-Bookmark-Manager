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
using static Youtube_Storage_2.MainWindow;

namespace Youtube_Storage_2
{
    /// <summary>
    /// Interaction logic for AddTimeWindow.xaml
    /// </summary>
    public partial class AddTimeWindow : Window
    {
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        Link selected = new Link();
        public AddTimeWindow()
        {
            InitializeComponent();

            selected = parent.GetLinkBySelected((MainWindow.Transfer)parent.FolderMenuList.SelectedItem);
        }

        //calculates the video time in seconds(colon seperated time)
        int CalculateTime(string time)
        {
            int finalTime = 0;
            int colonCount = 0;
            int currentNum = 0;

            foreach (char c in time)
            {
                if (c == ':')
                {
                    colonCount++;
                }
            }

            foreach (char c in time)
            {
                if (c == ':')
                {
                    finalTime += (int)(currentNum * (Math.Pow(60, colonCount)));
                    colonCount--;
                    currentNum = 0;
                }

                else
                {
                    int.TryParse($"{currentNum}{char.ToString(c)}", out currentNum);
                }
            }

            return finalTime + currentNum;
        }

        private void TextPressedEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (selected.LinkStr.Contains("&t="))
                {
                    int index = selected.LinkStr.IndexOf("&t=");

                    selected.LinkStr = selected.LinkStr.Substring(0, index);
                }

                string time = TimeText.Text;

                selected.LinkStr += $"&t={CalculateTime(time)}s";

                this.Close();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Keyboard.Focus(TimeText);
        }
    }
}
