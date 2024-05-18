using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        //Transfers a folder or link from an object to the format needed by the on screen list
        struct Transfer
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

            //TESTING
            currentFolder.AddFolder("JAKE");

            RefreshFolderList();
            TESTTEXTBLOCK.Text = ((Transfer)FolderMenuList.Items[0]).Type;
            //TESTING
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

        private void MenuItem_Click()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void DoubleClickFolderItem(object sender, MouseButtonEventArgs e)
        {
            //TODO NEXT TIME REMEMBEEEER
        }
    }
}
