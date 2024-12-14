using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Youtube_Storage_2
{
    [Serializable]
    public class Folder
    {
        public List<Folder> folders = new List<Folder>();
        public List<Link> links = new List<Link>();
        [XmlIgnore]
        public List<Link> allLinks = new List<Link>(); //Only used by the main folder. Contains all links of all folders
        [XmlIgnore]
        public Folder Parent { get; set; } = null;
        public string Name { get; set; } = "";
        public bool Hidden { get; set; } = false;

        public List<Folder> GetFolders() 
        { 
            return folders;
        }

        public List<Link> GetLinks() 
        {  
            return links;
        }

        public List<Link> GetAllLinks()
        {
            return allLinks;
        }

        public void AddFolder(string name)
        {
            Folder folder = new Folder();

            folder.Parent = this;
            folder.Name = name;

            folders.Add(folder);
        }

        public void AddFolder(Folder folder)
        {
            folder.Parent = this;

            folders.Add(folder);
        }

        public void RemoveFolder(int index)
        {
            folders.RemoveAt(index);
        }

        public void HideFolder(int index)
        {
            folders[index].Hidden = true;
        }

        public void RestoreFolder(int index)
        {
            folders[index].Hidden = false;
        }

        private void SendToAllLinks(Link link)
        {
            Folder folder = this;

            //Return to the main folder
            while(folder.Parent != null)
            {
                folder = folder.Parent;
            }

            folder.AddToAllLinks(link);
        }

        public void AddToAllLinks(Link link)
        {
            allLinks.Add(link);
        }

        public void AddLink(string name, string linkStr, string note)
        {
            Link link = new Link();

            link.Name = name;
            link.LinkStr = linkStr;
            link.Note = note;
            link.Parent = this;

            links.Add(link);
            SendToAllLinks(link);
        }

        public void AddLink(Link link, bool addToAllLinks = true)
        {
            link.Parent = this;

            links.Add(link);
            if (addToAllLinks)
            {
                SendToAllLinks(link);
            }
        }

        public void RemoveLink(int index)
        {
            links.RemoveAt(index);
        }

        public void RemoveFromAllLinks(Link link)
        {
            Folder folder = this;

            //Return to the main folder
            while (folder.Parent != null)
            {
                folder = folder.Parent;
            }

            folder.allLinks.Remove(link);
        }

        public void HideLink(int index)
        {
            links[index].Hidden = true;
        }

        public void RestoreLink(int index)
        {
            links[index].Hidden = false;
        }

        public void DeserializeFolders()
        {
            foreach(Folder folder in folders)
            {
                folder.Parent = this;
                folder.DeserializeFolders();
            }
            foreach(Link link in links)
            {
                link.Parent = this;
                SendToAllLinks(link);
            }

            allLinks.Reverse();
        }
    }
}
