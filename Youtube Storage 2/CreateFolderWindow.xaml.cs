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
    /// Interaction logic for CreateFolderWindow.xaml
    /// </summary>
    public partial class CreateFolderWindow : Window
    {
        bool edit;
        bool nameReset = false;
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        Folder selected = new Folder();

        public CreateFolderWindow(bool edit)
        {
            InitializeComponent();
            this.edit = edit;

            //Sets the object to edit
            if (parent.FolderMenuList.SelectedItem != null && edit)
            {
                selected = parent.GetCurrentFolder().GetFolders()[int.Parse(((MainWindow.Transfer)parent.FolderMenuList.SelectedItem).Index)];
            }

            if (edit)
            {
                NameText.Text = selected.Name;
            }
        }

        private void TextClicked(object sender, MouseButtonEventArgs e)
        {
            if (!nameReset)
            {
                if (NameText.Text == "Name" && !edit)
                {
                    NameText.Text = "";
                    nameReset = true;
                }
                else if (NameText.Text == selected.Name && edit)
                {
                    NameText.Text = "";
                    nameReset = true;
                }
            }
        }

        private void TextPressedEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (edit == false)
                {
                    parent.GetCurrentFolder().AddFolder(NameText.Text);
                }
                else
                {
                    selected.Name = NameText.Text;
                }

                this.Close();
            }
            else
            {
                if (NameText.Text == "Name" && !edit)
                {
                    NameText.Text = "";
                }
                else if (NameText.Text == selected.Name && edit)
                {
                    NameText.Text = "";
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Keyboard.Focus(NameText);
        }
    }
}
