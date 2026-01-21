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
    /// Interaction logic for EditAlternateNames.xaml
    /// </summary>
    public partial class EditAlternateNames : Window
    {
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        Folder selectedFolder = new Folder();
        Link selectedLink = new Link();
        string selectedType = "";

        public EditAlternateNames()
        {
            InitializeComponent();

            if(parent.FolderMenuList.SelectedItem != null)
            {
                selectedType = ((MainWindow.Transfer)parent.FolderMenuList.SelectedItem).Type;
            }

            if (parent.FolderMenuList.SelectedItem != null && selectedType == "F")
            {
                selectedFolder = parent.GetCurrentFolder().GetFolders()[int.Parse(((MainWindow.Transfer)parent.FolderMenuList.SelectedItem).Index)];
                Text.Text = selectedFolder.alternateNames;
            }
            else if(parent.FolderMenuList.SelectedItem != null && selectedType == "L")
            {
                selectedLink = parent.GetLinkBySelected((MainWindow.Transfer)parent.FolderMenuList.SelectedItem);
                Text.Text = selectedLink.alternateNames;
            }
        }

        private void TextPressedEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (selectedType == "F")
                {
                    selectedFolder.alternateNames = Text.Text;
                }
                else if (selectedType == "L")
                {
                    selectedLink.alternateNames = Text.Text;
                }

                this.Close();
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Keyboard.Focus(Text);
        }
    }
}
