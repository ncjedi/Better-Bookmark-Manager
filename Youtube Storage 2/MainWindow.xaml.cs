using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Youtube_Storage_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Folder currentFolder;
        Settings settings = new Settings();

        public Folder GetCurrentFolder()
        {
            return currentFolder;
        }

        public void setCurrentFolder(Folder value)
        {
            currentFolder = value;
        }

        //Transfers a folder or link from an object to the format needed by the on screen list
        public struct Transfer
        {
            public string ItemName { get; set; }
            public string ItemImage { get; set; }
            public string Type { get; set; }
            public string Hidden { get; set; }
            public string Index { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            /*TESTING
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddFolder("JAKE");
            currentFolder.AddLink("JIM", "https://en.wikipedia.org/wiki/Hamburger_University", "");
            TESTING*/

            RefreshFolderList();
        }

        void LoadData()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Folder));

            if (File.Exists("./Data/data.xml"))
            {
                StreamReader stream = new StreamReader("./Data/data.xml");

                currentFolder = (Folder)xmlSerializer.Deserialize(stream);
            }
            else
            {
                currentFolder = new Folder();

                Directory.CreateDirectory("./Data");
                StreamWriter stream = new StreamWriter("./Data/data.xml");

                xmlSerializer.Serialize(stream, currentFolder);
            }
        }

        public void RefreshFolderList()
        {
            Transfer transfer = new Transfer();
            Folder folder;
            Link link;

            FolderMenuList.Items.Clear();

            if(currentFolder.Parent != null) 
            {
                transfer.ItemName = "(Back)";
                transfer.ItemImage = "C:\\Users\\Chris\\Pictures\\8d7d52621ddef15795b1ae815a8bc5a3.jpg";
                transfer.Type = "P";
                transfer.Hidden = "F";
                transfer.Index = "0";

                FolderMenuList.Items.Add(transfer);
            }

            for (int i = 0; i < currentFolder.GetFolders().Count; i++)
            {
                folder = currentFolder.GetFolders()[i];

                transfer.ItemName = folder.Name;
                transfer.ItemImage = "C:\\Users\\Chris\\Pictures\\8d7d52621ddef15795b1ae815a8bc5a3.jpg";
                transfer.Type = "F";
                transfer.Hidden = "F";
                transfer.Index = i.ToString();

                FolderMenuList.Items.Add(transfer);
            }

            for (int i = 0; i < currentFolder.GetLinks().Count; i++)
            {
                link = currentFolder.GetLinks()[i];

                transfer.ItemName = link.Name;
                transfer.ItemImage = "C:\\Users\\Chris\\Pictures\\8d7d52621ddef15795b1ae815a8bc5a3.jpg";
                transfer.Type = "L";
                transfer.Hidden = "F";
                transfer.Index = i.ToString();

                FolderMenuList.Items.Add(transfer);
            }
        }

        //Filters the list based on search
        void SearchFolderList(string search = "")
        {
            RefreshFolderList();

            if(search == "")
            {
                return;
            }

            List<Transfer> items = new List<Transfer>();

            foreach(Transfer item in FolderMenuList.Items)
            {
                items.Add(item);
            }

            foreach (Transfer item in items)
            {
                if(!item.ItemName.ToLower().Contains(search.ToLower()) || item.Type == "P")
                {
                    FolderMenuList.Items.Remove(item);
                }
            }
        }

        private void MenuItem_Click()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Transfer selected = new Transfer();

            if (FolderMenuList.SelectedItem != null)
            {
                selected = (Transfer)FolderMenuList.SelectedItem;
            }
            else
            {
                return;
            }

            if (selected.Type == "L")
            {
                NoteTextBox.Text = currentFolder.GetLinks()[int.Parse(selected.Index)].Note;
            }
            else
            {
                NoteTextBox.Text = "";
            }
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void DoubleClickFolderItem(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            //If the parent directory is double clicked return to it
            if (selected.Type == "P")
            {
                currentFolder = currentFolder.Parent;
                RefreshFolderList();
            }

            //If a folder is clicked go into it
            else if (selected.Type == "F")
            {
                currentFolder = currentFolder.GetFolders()[int.Parse(selected.Index)];
                RefreshFolderList();
            }

            //If a link is clicked open it in the selected browser
            else if (selected.Type == "L")
            {
                Process.Start(settings.BrowserPath, currentFolder.GetLinks()[int.Parse(selected.Index)].LinkStr);
            }
        }

        private void FolderListContextMenuOpen(object sender, ContextMenuEventArgs e)
        {
            Transfer selected = new Transfer();

            //Disables edit button
            ((MenuItem)FolderMenuList.ContextMenu.Items[2]).IsEnabled = false;

            //Disables and hides the set link button
            ((MenuItem)FolderMenuList.ContextMenu.Items[3]).IsEnabled = false;
            ((MenuItem)FolderMenuList.ContextMenu.Items[3]).Visibility = Visibility.Collapsed;

            if (FolderMenuList.SelectedItem != null)
            {
                selected = (Transfer)FolderMenuList.SelectedItem;
            }
            else
            {
                return;
            }

            if(selected.Type == "F")
            {
                ((MenuItem)FolderMenuList.ContextMenu.Items[2]).IsEnabled = true; //Enable edit button
            }

            else if (selected.Type == "L")
            {
                ((MenuItem)FolderMenuList.ContextMenu.Items[2]).IsEnabled = true; //Enable edit button

                //Enables the set link button
                ((MenuItem)FolderMenuList.ContextMenu.Items[3]).IsEnabled = true;
                ((MenuItem)FolderMenuList.ContextMenu.Items[3]).Visibility = Visibility.Visible;
            }
        }

        private void MenuItem_Click_NewFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderWindow createFolderWindow = new CreateFolderWindow(false);

            createFolderWindow.ShowDialog();

            RefreshFolderList();
        }

        private void MenuItem_Click_NewLink(object sender, RoutedEventArgs e)
        {
            CreateLinkWindow createLinkWindow = new CreateLinkWindow(false);

            createLinkWindow.ShowDialog();

            RefreshFolderList();
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;
            if (selected.Type == "F")
            {
                CreateFolderWindow createFolderWindow = new CreateFolderWindow(true);

                createFolderWindow.ShowDialog();
            }
            else if(selected.Type == "L")
            {
                CreateLinkWindow createLinkWindow = new CreateLinkWindow(true);

                createLinkWindow.ShowDialog();
            }

                RefreshFolderList();
        }

        private void MenuItem_Click_SetLink(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            currentFolder.GetLinks()[int.Parse(selected.Index)].SetLinkStr(StaticFunctions.GetActiveTabUrl());
        }

        private void SearchInput(object sender, TextChangedEventArgs e)
        {
            SearchFolderList(SearchTextBox.Text);
        }
    }
}
