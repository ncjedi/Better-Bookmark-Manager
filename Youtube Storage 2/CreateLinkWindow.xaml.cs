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
    /// Interaction logic for CreateLinkWindow.xaml
    /// </summary>
    public partial class CreateLinkWindow : Window
    {
        bool edit;
        bool noteTyped;
        MainWindow parent = (MainWindow)Application.Current.MainWindow;
        Link selected = new Link();

        public CreateLinkWindow(bool edit)
        {
            InitializeComponent();

            this.edit = edit;

            if (parent.FolderMenuList.SelectedItem != null)
            {
                selected = parent.GetCurrentFolder().GetLinks()[int.Parse(((MainWindow.Transfer)parent.FolderMenuList.SelectedItem).Index)];
            }

            if (edit)
            {
                NameText.Text = selected.Name;
                LinkText.Text = selected.LinkStr;
                NoteText.Text = selected.Note;

                this.Title = "Edit Link";
                CreateButton.Content = "Finish";

                noteTyped = true;
            }
            else
            {
                noteTyped = false;
            }
        }

        private void NameInFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*if (NameText.Text == "Name" && !edit)
            {
                NameText.Text = "";
            }
            else if (NameText.Text == selected.Name && edit)
            {
                NameText.Text = "";
            }*/
        }

        private void NameEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                Keyboard.Focus(LinkText);
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

        private void LinkInFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*if (LinkText.Text == "Link" && !edit)
            {
                LinkText.Text = "";
            }
            else if (LinkText.Text == selected.LinkStr && edit)
            {
                LinkText.Text = "";
            }*/
        }

        private void LinkEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                Keyboard.Focus(NoteText);
            }
            else
            {
                if (LinkText.Text == "Link" && !edit)
                {
                    LinkText.Text = "";
                }
                else if (LinkText.Text == selected.LinkStr && edit)
                {
                    LinkText.Text = "";
                }
            }
        }

        private void NoteInFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*if (NoteText.Text == "Note" && !edit)
            {
                NoteText.Text = "";
            }
            else if (NoteText.Text == selected.Note && edit)
            {
                NoteText.Text = "";
            }*/
        }

        private void NoteEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindow parent = (MainWindow)Application.Current.MainWindow;

                if (!edit)
                {
                    if(!noteTyped)
                    {
                        NoteText.Text = "";
                    }

                    parent.GetCurrentFolder().AddLink(NameText.Text, LinkText.Text, NoteText.Text);
                }
                else
                {
                    selected.Name = NameText.Text;
                    selected.LinkStr = LinkText.Text;
                    selected.Note = NoteText.Text;
                }

                this.Close();
            }
            else
            {
                if (NoteText.Text == "Note" && !edit)
                {
                    noteTyped = true;
                    NoteText.Text = "";
                }
                else if (NoteText.Text == selected.Note && edit)
                {
                    NoteText.Text = "";
                }
            }
        }

        private void BrowserButtonClick(object sender, RoutedEventArgs e)
        {
            LinkText.Text = StaticFunctions.GetActiveTabUrl();
        }

        private void CreateClicked(object sender, RoutedEventArgs e)
        {
            MainWindow parent = (MainWindow)Application.Current.MainWindow;

            if (!edit)
            {
                parent.GetCurrentFolder().AddLink(NameText.Text, LinkText.Text, NoteText.Text);
            }
            else
            {
                selected.Name = NameText.Text;
                selected.LinkStr = LinkText.Text;
                selected.Note = NoteText.Text;
            }

            this.Close();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Keyboard.Focus(NameText);
        }
    }
}
