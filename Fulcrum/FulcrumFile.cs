using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fulcrum
{
    enum EntryType
    {
        EntryRoot,
        EntryFile,
        EntryDir
    }

    class FileEntry
    {
        public EntryType Type { get; set; }
        public string Name { get; set; }
        public List<FileEntry> Children { get; set; }

        public long Offset { get; set; }
        public long Len { get; set; }

        public override string ToString()
        {
            return Type.ToString() + " " + Name + " " + Children.Count + " children";
        }
    }

    class FulcrumFile
    {

        FileEntry root;

        const int MAX_PATH = 260;

        public void CreateFromDirectory(string directory)
        {
            root = new FileEntry();
            root.Name = directory;
            root.Type = EntryType.EntryRoot;
            root.Children = new List<FileEntry>();
            foreach (string fsEntry in Directory.EnumerateFileSystemEntries(directory))
            {
                root.Children.Add(CreateFileEntry(directory, fsEntry));
            }
        }

        private FileEntry CreateFileEntry(string rootPath, string path)
        {
            FileEntry entry = new FileEntry();
            entry.Children = new List<FileEntry>();
            FileAttributes attrs = File.GetAttributes(path);
            StringBuilder builder = new StringBuilder(MAX_PATH);
            PathRelativePathTo(builder, rootPath, FileAttributes.Directory, path, attrs);
            entry.Name = builder.ToString();
            if (attrs.HasFlag(FileAttributes.Directory))
            {
                entry.Type = EntryType.EntryDir;
                foreach (string fsEntry in Directory.EnumerateFileSystemEntries(path))
                {
                    entry.Children.Add(CreateFileEntry(rootPath, fsEntry));
                }
            }
            else
            {
                entry.Type = EntryType.EntryFile;
            }
            return entry;
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        static extern bool PathRelativePathTo(
            [Out] StringBuilder pszPath,
            [In] string pszFrom,
            [In] FileAttributes dwAttrFrom,
            [In] string pszTo,
            [In] FileAttributes dwAttrTo
        );

        public void SaveToFile(string file)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(file, FileMode.Create)))
            {
                CopyFileData(writer, root);
                long fileTableOffset = writer.BaseStream.Position;
                SaveFileEntry(writer, root);
                writer.Write(fileTableOffset);
            }
        }

        private void CopyFileData(BinaryWriter writer, FileEntry entry)
        {
            string inFilePath = Path.Combine(root.Name, entry.Name);
            if (entry.Type == EntryType.EntryFile)
            {
                using (FileStream inFile = new FileStream(inFilePath, FileMode.Open))
                {
                    entry.Offset = writer.BaseStream.Position;
                    inFile.CopyTo(writer.BaseStream);
                    entry.Len = writer.BaseStream.Position - entry.Offset;
                }
            }
            else
            {
                foreach (FileEntry child in entry.Children)
                {
                    CopyFileData(writer, child);
                }
            }
        }

        private void SaveFileEntry(BinaryWriter writer, FileEntry entry)
        {
            writer.Write((int)entry.Type);
            writer.Write(entry.Name);
            writer.Write(entry.Offset);
            writer.Write(entry.Len);
            writer.Write(entry.Children.Count);
            foreach (FileEntry child in entry.Children)
            {
                SaveFileEntry(writer, child);
            }
        }

    }
}
