using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Storage_2
{
    public class Folder
    {
        List<Folder> folders = new List<Folder>();
        List<Link> links = new List<Link>();
        Folder parent = null;
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

        public void AddFolder(string name)
        {
            Folder folder = new Folder();

            folder.parent = this;
            folder.Name = name;

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

        public void AddLink(string name, string linkStr, string note)
        {
            Link link = new Link();

            link.Name = name;
            link.LinkStr = linkStr;
            link.Note = note;

            links.Add(link);
        }

        public void RemoveLink(int index)
        {
            links.RemoveAt(index);
        }

        public void HideLink(int index)
        {
            links[index].Hidden = true;
        }

        public void RestoreLink(int index)
        {
            links[index].Hidden = false;
        }


    }
}
