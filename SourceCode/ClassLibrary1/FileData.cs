using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects.Extensions;

namespace BusinessObjects
{
    public class FileData
    {
        #region Properties

        public int? ID;
        public string Path;

        public string Root;
        public bool IsNetwork;
        public bool IsLocal;

        public string DirectoryName;
        public string CompressedContainerName;
        public string Name;
        public string Extension;
        public bool ReadOnly;
        FileAttributes Attributes;

        public bool IsCompressedContainer;
        public bool IsContained;

        public long Size;
        public double KB;
        public double MB;
        public double GB;

        public DateTime? CreateTime;
        public DateTime? CreateTimeUtc;
        public DateTime? LastAccessTime;
        public DateTime? LastAccessTimeUtc;
        public DateTime? LastWriteTime;
        public DateTime? LastWriteTimeUtc;

        #endregion

        #region Constructors

        public FileData(FileInfo info)
        {
            if (!info.Exists)
                throw new FileNotFoundException(String.Format("File not found: {0}", info.FullName));

            Path = info.FullName;

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            DirectoryName = info.DirectoryName;
            Name = info.Name;
            Extension = info.Extension;
            ReadOnly = info.IsReadOnly;

            Size = info.Length;
            KB = GetKB();
            MB = GetMB();
            GB = GetGB();

            FileAttributes Attributes = info.Attributes;

            try
            {
                CreateTime = info.CreationTime;
                CreateTimeUtc = info.CreationTimeUtc;
            } catch { }
            try
            {
                LastAccessTime = info.LastAccessTime;
                LastAccessTimeUtc = info.LastAccessTimeUtc;
            } catch { }
            try
            {
                LastWriteTime = info.LastWriteTime;
                LastWriteTimeUtc = info.LastWriteTimeUtc;
            }
            catch { }
        }

        public FileData(DataRow row)
        {
            //Load from Database Row
            ID = row["ID"].ToInt();
        }

        #endregion

        #region Methods

        public double GetKB()
        {
            return ((double)Size) / 1024;
        }

        public double GetMB()
        {
            return ((double)Size) / 1024 / 1024;
        }

        public double GetGB()
        {
            return ((double)Size) / 1024 / 1024 / 1024;
        }
        
        #endregion
    }
}
