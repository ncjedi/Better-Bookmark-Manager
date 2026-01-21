using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Youtube_Storage_2
{
    public class Link
    {
        [XmlIgnore]
        public Folder Parent { get; set; } = null;
        public string Name { get; set; } = "";
        public string LinkStr { get; set; } = "";
        public string Note { get; set; } = "";
        public bool Hidden { get; set; } = false;

        public List<string> history = new List<string>();

        public string alternateNames { get; set; } = "";

        public List<string> getHistory() { return history; }

        public void SetLinkStr(string link)
        {
            if (link != LinkStr)
            {
                history.Add(LinkStr);
            }
            LinkStr = link;
        }

        public string GetFullName()
        {
            return Name + alternateNames;
        }
    }
}
