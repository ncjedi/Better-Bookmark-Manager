using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Storage_2
{
    public static class Clipboard
    {
        static int itemIndex = 0;
        static Folder clipboardFolder;
        static Link clipboardLink;

        static Folder cutFromFolder;

        public static bool IsEmpty()
        {
            if (clipboardFolder == null && clipboardLink == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CutLink(Link link, int linkIndex, Folder currentFolder)
        {
            ResetClipboard();
            clipboardLink = link;
            itemIndex = linkIndex;
            cutFromFolder = currentFolder;
        }

        public static void CutFolder(Folder folder, int folderIndex, Folder currentFolder)
        {
            ResetClipboard();
            clipboardFolder = folder;
            itemIndex = folderIndex;
            cutFromFolder = currentFolder;
        }

        public static void CopyLink(Link link)
        {
            ResetClipboard();
            clipboardLink = link;
        }

        public static void CopyFolder(Folder folder)
        {
            ResetClipboard();
            clipboardFolder = folder;
        }

        static void CutRemoveItem()
        {
            if (clipboardFolder != null)
            {
                cutFromFolder.RemoveFolder(itemIndex);
            }
            else if (clipboardLink != null)
            {
                cutFromFolder.RemoveLink(itemIndex);
            }
        }

        public static void PasteItem(Folder currentFolder)
        {
            if (clipboardFolder != null)
            {
                currentFolder.AddFolder(clipboardFolder);
            }
            else if (clipboardLink != null)
            {
                currentFolder.AddLink(clipboardLink, false);
            }

            if (cutFromFolder != null)
            {
                CutRemoveItem();
            }

            ResetClipboard();
        }

        static void ResetClipboard()
        {
            clipboardFolder = null;
            clipboardLink = null;
            cutFromFolder = null;
        }
    }
}
