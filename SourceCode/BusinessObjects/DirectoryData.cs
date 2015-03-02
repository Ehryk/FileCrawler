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
    public class DirectoryData
    {
        #region Properties

        public int? ID;
        public string Path;
        
        public string Root;
        public bool IsLocal;
        public bool IsNetwork;

        public string Name;
        public string ParentName;
        public string Extension;
        public bool ReadOnly;
        FileAttributes Attributes;

        public DateTime? CreateTime;
        public DateTime? CreateTimeUtc;
        public DateTime? LastAccessTime;
        public DateTime? LastAccessTimeUtc;
        public DateTime? LastWriteTime;
        public DateTime? LastWriteTimeUtc;

        #endregion

        #region Constructors

        public DirectoryData(DirectoryInfo info)
        {
            if (!info.Exists)
                throw new DirectoryNotFoundException(String.Format("Directory not found: {0}", info.FullName));

            Path = info.FullName;

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            Name = info.Name;
            ParentName = info.Parent.Name;
            Extension = info.Extension;

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

        public DirectoryData(DataRow row)
        {
            //Load from Database Row
            ID = row["ID"].ToInt();
        }

        #endregion

        #region Methods
        
        #endregion
    }
}
