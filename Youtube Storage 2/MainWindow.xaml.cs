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
using static Youtube_Storage_2.MainWindow;

namespace Youtube_Storage_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Folder currentFolder;
        public Settings settings = new Settings();

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
            LoadSettings();

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

                stream.Close();

                currentFolder.DeserializeFolders();
            }
            else
            {
                currentFolder = new Folder();

                Directory.CreateDirectory("./Data");
                StreamWriter stream = new StreamWriter("./Data/data.xml");

                xmlSerializer.Serialize(stream, currentFolder);

                stream.Close();
            }
        }

        void LoadSettings()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));

            if (File.Exists("./Data/settings.xml"))
            {
                StreamReader stream = new StreamReader("./Data/settings.xml");

                settings = (Settings)xmlSerializer.Deserialize(stream);

                stream.Close();
            }
            else
            {
                settings = new Settings();

                StreamWriter stream = new StreamWriter("./Data/settings.xml");

                xmlSerializer.Serialize(stream, settings);

                stream.Close();
            }
        }

        void SaveData()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Folder));

            Folder saveFolder = currentFolder;

            while(saveFolder.Parent != null)
            {
                saveFolder = saveFolder.Parent;
            }

            StreamWriter stream = new StreamWriter("./Data/data.xml");
            xmlSerializer.Serialize(stream, saveFolder);
            stream.Close();
        }

        void SaveSettings()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));

            StreamWriter stream = new StreamWriter("./Data/settings.xml");
            xmlSerializer.Serialize(stream, settings);
            stream.Close();
        }

        //Shows or hides deleted items based on the Show Deleted checkbox
        public void HideDeleted()
        {
            List<Transfer> items = new List<Transfer>();

            foreach (Transfer item in FolderMenuList.Items)
            {
                items.Add(item);
            }

            foreach (Transfer item in items)
            {
                if(item.Type == "P")
                {
                    continue;
                }

                if (item.Hidden == "T" && ShowDeletedCheck.IsChecked == false)
                {
                    FolderMenuList.Items.Remove(item);
                }
                else if(item.Hidden == "F" && ShowDeletedCheck.IsChecked == true)
                {
                    FolderMenuList.Items.Remove(item);
                }
            }
        }

        void NormalRefreshList()
        {
            Transfer transfer = new Transfer();
            Folder folder;
            Link link;

            if (currentFolder.Parent != null)
            {
                transfer.ItemName = "(Back)";
                transfer.ItemImage = settings.ParentIconPath;
                transfer.Type = "P";
                transfer.Hidden = "F";
                transfer.Index = "0";

                FolderMenuList.Items.Add(transfer);
            }

            for (int i = 0; i < currentFolder.GetFolders().Count; i++)
            {
                folder = currentFolder.GetFolders()[i];

                transfer.ItemName = folder.Name;
                transfer.ItemImage = settings.FolderIconPath;
                transfer.Type = "F";

                if (folder.Hidden)
                {
                    transfer.Hidden = "T";
                }
                else
                {
                    transfer.Hidden = "F";
                }
                transfer.Index = i.ToString();

                FolderMenuList.Items.Add(transfer);
            }

            for (int i = 0; i < currentFolder.GetLinks().Count; i++)
            {
                link = currentFolder.GetLinks()[i];

                transfer.ItemName = link.Name;
                transfer.ItemImage = settings.LinkIconPath;
                transfer.Type = "L";

                if (link.Hidden)
                {
                    transfer.Hidden = "T";
                }
                else
                {
                    transfer.Hidden = "F";
                }

                transfer.Index = i.ToString();

                FolderMenuList.Items.Add(transfer);
            }
        }

        void AllLinksRefreshList()
        {
            Transfer transfer = new Transfer();
            Link link;

            while (currentFolder.Parent != null)
            {
                currentFolder = currentFolder.Parent;
            }

            for (int i = 0; i < currentFolder.GetAllLinks().Count; i++)
            {
                link = currentFolder.GetAllLinks()[i];

                transfer.ItemName = link.Name;
                transfer.ItemImage = settings.LinkIconPath;
                transfer.Type = "L";

                if (link.Hidden)
                {
                    transfer.Hidden = "T";
                }
                else
                {
                    transfer.Hidden = "F";
                }

                transfer.Index = i.ToString();

                FolderMenuList.Items.Add(transfer);
            }
        }

        public void RefreshFolderList()
        {
            int refreshType;

            if (ShowAllLinksCheck.IsChecked == true)
            {
                refreshType = 1;
            }
            else
            {
                refreshType = 0;
            }

            FolderMenuList.Items.Clear();

            //Load data into the list based on the refresh type
            switch(refreshType)
            {
                case 0:
                    NormalRefreshList();
                    break;
                case 1:
                    AllLinksRefreshList();
                    break;
            }

            //Filter the list
            SearchFolderList();
            HideDeleted();
        }

        //Filters the list based on search
        void SearchFolderList()
        {
            string search = SearchTextBox.Text;

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
                NoteTextBox.Text = GetLinkBySelected(selected).Note;
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
            Transfer selected = new Transfer();

            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }

            if (FolderMenuList.SelectedItem != null)
            {
                selected = (Transfer)FolderMenuList.SelectedItem;
            }

            //If the parent directory is double clicked return to it
            if (selected.Type == "P")
            {
                currentFolder = currentFolder.Parent;
                RefreshFolderList();
                SearchTextBox.Text = "";
            }

            //If a folder is clicked go into it
            else if (selected.Type == "F")
            {
                currentFolder = currentFolder.GetFolders()[int.Parse(selected.Index)];
                RefreshFolderList();
                SearchTextBox.Text = "";
            }

            //If a link is clicked open it in the selected browser
            else if (selected.Type == "L")
            {
                if(!File.Exists(settings.BrowserPath))
                {
                    return;
                }
                Process.Start(settings.BrowserPath, GetLinkBySelected(selected).LinkStr);
            }
        }

        ///<summary>
        ///Button Strings: addFolder, addLink, edit, cut, copy, paste, delete, setLink, restoreDeleted, perminatelyDelete, addTime
        ///</summary>
        public void ContextMenuEnableDisable(string button, bool enableChoice, bool visibleChoice = true)
        {
            Visibility visibility;

            if (visibleChoice)
            {
                visibility = Visibility.Visible;
            }
            else
            {
                visibility = Visibility.Collapsed;
            }

            switch(button)
            {
                case "addFolder":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[0]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[0]).Visibility = visibility;
                        break;
                    }
                case "addLink":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[1]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[1]).Visibility = visibility;
                        break;
                    }
                case "edit":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[2]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[2]).Visibility = visibility;
                        break;
                    }
                case "cut":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[3]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[3]).Visibility = visibility;
                        break;
                    }
                case "copy":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[4]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[4]).Visibility = visibility;
                        break;
                    }
                case "paste":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[5]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[5]).Visibility = visibility;
                        break;
                    }
                case "delete":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[6]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[6]).Visibility = visibility;
                        break;
                    }
                case "setLink":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[7]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[7]).Visibility = visibility;
                        break;
                    }
                case "restoreDeleted":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[8]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[8]).Visibility = visibility;
                        break;
                    }
                case "perminatelyDelete":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[9]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[9]).Visibility = visibility;
                        break;
                    }
                case "addTime":
                    {
                        ((MenuItem)FolderMenuList.ContextMenu.Items[10]).IsEnabled = enableChoice;
                        ((MenuItem)FolderMenuList.ContextMenu.Items[10]).Visibility = visibility;
                        break;
                    }
            }
        }

        private void FolderListContextMenuOpen(object sender, ContextMenuEventArgs e)
        {
            Transfer selected = new Transfer();

            //Disables edit button
            ContextMenuEnableDisable("edit", false);

            //Disables cut button
            ContextMenuEnableDisable("cut", false);

            //Disables copy button
            ContextMenuEnableDisable("copy", false);

            //Disables paste button
            ContextMenuEnableDisable("paste", false);

            //Disables delete button
            ContextMenuEnableDisable("delete", false);

            //Disables and hides the set link button
            ContextMenuEnableDisable("setLink", false, false);

            //Disables and hides the restore deleted button
            ContextMenuEnableDisable("restoreDeleted", false, false);

            //Disables and hides the perminately delete button
            ContextMenuEnableDisable("perminatelyDelete", false, false);

            //Disables and hides the add time button
            ContextMenuEnableDisable("addTime", false, false);

            if (!Clipboard.IsEmpty())
            {
                ContextMenuEnableDisable("paste", true); //Enable paste button
            }

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
                ContextMenuEnableDisable("edit", true); //Enable edit button
                ContextMenuEnableDisable("delete", true); //Enable delete button
                ContextMenuEnableDisable("cut", true); //Enable cut button
                ContextMenuEnableDisable("copy", true); //Enable copy button
            }

            else if (selected.Type == "L")
            {
                ContextMenuEnableDisable("edit", true); //Enable edit button
                ContextMenuEnableDisable("delete", true); //Enable delete button
                ContextMenuEnableDisable("cut", true); //Enable cut button
                ContextMenuEnableDisable("copy", true); //Enable copy button

                //Enables the set link button
                ContextMenuEnableDisable("setLink", true, true);

                //Enables the add time button
                ContextMenuEnableDisable("addTime", true, true);
            }

            if(ShowDeletedCheck.IsChecked == true && selected.Type != "P")
            {
                ContextMenuEnableDisable("delete", false); //Redisable delete button

                //Enables the restore deleted button
                ContextMenuEnableDisable("restoreDeleted", true, true);

                //Enables the perminately delete button
                ContextMenuEnableDisable("perminatelyDelete", true, true);
            }
        }

        private void MenuItem_Click_NewFolder(object sender, RoutedEventArgs e)
        {
            CreateFolderWindow createFolderWindow = new CreateFolderWindow(false);

            createFolderWindow.ShowDialog();

            SaveData();
            RefreshFolderList();
        }

        private void MenuItem_Click_NewLink(object sender, RoutedEventArgs e)
        {
            CreateLinkWindow createLinkWindow = new CreateLinkWindow(false);

            createLinkWindow.ShowDialog();

            SaveData();
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

            SaveData();
            RefreshFolderList();
        }

        private void MenuItem_Click_SetLink(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            GetLinkBySelected(selected).SetLinkStr(StaticFunctions.GetActiveTabUrl());

            SaveData();
        }

        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            if(selected.Type == "F")
            {
                currentFolder.GetFolders()[int.Parse(selected.Index)].Hidden = true;
            }
            else if(selected.Type == "L")
            {
                GetLinkBySelected(selected).Hidden = true;
            }

            SaveData();
            RefreshFolderList();
        }

        //returns the link represented by the transfer passed into the function. 
        public Link GetLinkBySelected(Transfer selected)
        {
            if(ShowAllLinksCheck.IsChecked == true)
            {
                return currentFolder.GetAllLinks()[int.Parse(selected.Index)];
            }
            else
            {
                return currentFolder.GetLinks()[int.Parse(selected.Index)];
            }
        }

        public List<Link> GetLinkList()
        {
            if (ShowAllLinksCheck.IsChecked == true)
            {
                return currentFolder.GetAllLinks();
            }
            else
            {
                return currentFolder.GetLinks();
            }
        }

        void ImportUncategorizedLinks(string mainDirectory, Folder mainFolder)
        {
                foreach (string file in File.ReadAllLines($"{mainDirectory}\\aaaall63672\\AAAseries.txt"))
                {
                    if (file.Contains("-"))
                    {
                        continue;
                    }

                    bool editLink = false;
                    Link linkToCreate = new Link();
                    string[] contents;
                    string note = "";
                    string filePath = $"{mainDirectory}\\aaaall63672\\{file}.txt";
                    int i = 2;

                    linkToCreate.Name = file;

                    foreach (Link link in mainFolder.links)
                    {
                        if (link.Name == linkToCreate.Name)
                        {
                            linkToCreate = link;
                            editLink = true;
                            break;
                        }
                    }

                    contents = File.ReadAllLines(filePath);

                    linkToCreate.SetLinkStr(contents[0]);

                    while (i < contents.Length)
                    {
                        note += contents[i] + "\n";
                        i++;
                    }

                    linkToCreate.Note = note;

                    if (!editLink)
                    {
                        mainFolder.AddLink(linkToCreate);
                    }
                }
            }

        public void ImportPressed()
        {
            string mainDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\YoutubeStorage";
            Folder mainFolder = currentFolder;

            if(!Directory.Exists(mainDirectory))
            {
                return;
            }

            while (mainFolder.Parent != null)
            {
                mainFolder = mainFolder.Parent;
            }

            foreach (string directory in File.ReadAllLines($"{mainDirectory}\\AAApeeps.txt"))
            {
                bool editFolder = false;
                string directoryPath = $"{mainDirectory}\\{directory}";
                Folder folderToCreate = new Folder();
                folderToCreate.Name = directory;

                foreach (Folder folder in mainFolder.folders)
                {
                    if (folder.Name == folderToCreate.Name)
                    {
                        folderToCreate = folder;
                        editFolder = true;
                        break;
                    }
                }

                if (!editFolder)
                {
                    mainFolder.AddFolder(folderToCreate);
                }

                foreach (string file in File.ReadAllLines($"{directoryPath}\\AAAseries.txt"))
                {
                    if (System.IO.Path.GetFileNameWithoutExtension(file) == "AAAseries")
                    {
                        continue;
                    }

                    bool editLink = false;
                    Link linkToCreate = new Link();
                    string[] contents;
                    string note = "";
                    string filePath = $"{directoryPath}\\{file}.txt";
                    int i = 2;

                    linkToCreate.Name = file;

                    foreach (Link link in folderToCreate.links)
                    {
                        if (link.Name == linkToCreate.Name)
                        {
                            linkToCreate = link;
                            editLink = true;
                            break;
                        }
                    }

                    contents = File.ReadAllLines(filePath);

                    linkToCreate.SetLinkStr(contents[0]);

                    while (i < contents.Length)
                    {
                        note += contents[i] + "\n";
                        i++;
                    }

                    linkToCreate.Note = note;

                    if (!editLink)
                    {
                        folderToCreate.AddLink(linkToCreate);
                    }
                }

                ImportUncategorizedLinks(mainDirectory, mainFolder);
            }
        }

        private void SearchInput(object sender, TextChangedEventArgs e)
        {
            RefreshFolderList();
        }

        private void ShowDeletedCheck_Checked(object sender, RoutedEventArgs e)
        {
            RefreshFolderList();
        }

        private void ShowAllLinksCheck_Checked(object sender, RoutedEventArgs e)
        {
            RefreshFolderList();
        }

        private void MenuItem_Click_DeleteRestore(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            if (selected.Type == "F")
            {
                currentFolder.GetFolders()[int.Parse(selected.Index)].Hidden = false;
            }
            else if (selected.Type == "L")
            {
                GetLinkBySelected(selected).Hidden = false;
            }

            SaveData();
            RefreshFolderList();
        }

        private void MenuItem_Click_DeletePerminate(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            if (selected.Type == "F")
            {
                currentFolder.RemoveFolder(int.Parse(selected.Index));
            }
            else if (selected.Type == "L")
            {
                Link link = GetLinkBySelected(selected);

                currentFolder.RemoveFromAllLinks(link);
                link.Parent.links.Remove(link);
            }

            SaveData();
            RefreshFolderList();
        }

        private void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();

            settingsWindow.ShowDialog();

            SaveData();
            SaveSettings();
            RefreshFolderList();
        }

        private void MenuItem_Click_AddTime(object sender, RoutedEventArgs e)
        {
            AddTimeWindow addTimeWindow = new AddTimeWindow();
            addTimeWindow.ShowDialog();

            SaveData();
            RefreshFolderList();
        }

        private void MenuItem_Click_Cut(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            if (selected.Type == "F")
            {
                Clipboard.CutFolder(currentFolder.folders[int.Parse(selected.Index)], int.Parse(selected.Index), currentFolder);
            }
            else if (selected.Type == "L")
            {
                Clipboard.CutLink(currentFolder.links[int.Parse(selected.Index)], int.Parse(selected.Index), currentFolder);
            }
        }

        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            Transfer selected = (Transfer)FolderMenuList.SelectedItem;

            if (selected.Type == "F")
            {
                Clipboard.CopyFolder(currentFolder.folders[int.Parse(selected.Index)]);
            }
            else if (selected.Type == "L")
            {
                Clipboard.CopyLink(currentFolder.links[int.Parse(selected.Index)]);
            }
        }

        private void MenuItem_Click_Paste(object sender, RoutedEventArgs e)
        {
            Clipboard.PasteItem(currentFolder);

            RefreshFolderList();
        }
    }
}
