﻿using System;
using System.IO;
using System.Data;
using Common.Extensions;

namespace Common.Objects
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
        public bool ReadOnly;
        public FileAttributes Attributes;
        public bool Hidden { get { return Attributes.HasAttribute(FileAttributes.Hidden); } }

        public DateTime? CreateTime;
        public DateTime? CreateTimeUtc;
        public DateTime? LastAccessTime;
        public DateTime? LastAccessTimeUtc;
        public DateTime? LastWriteTime;
        public DateTime? LastWriteTimeUtc;

        public int FileCount;
        public long TotalSize;

        public decimal TotalKB { get { return TotalSize.GetKB(); } }
        public decimal TotalMB { get { return TotalSize.GetMB(); } }
        public decimal TotalGB { get { return TotalSize.GetGB(); } }

        #endregion

        #region Constructors

        public DirectoryData(string path)
        {
            path = FileExtensions.FixPath(path);

            Path = path;

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            string withTrailing = path.TrimEnd('\\') + "\\";
            Name = System.IO.Path.GetDirectoryName(withTrailing);
            ParentName = System.IO.Path.GetDirectoryName(Name);
        }

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

            if (info.Parent != null)
                ParentName = info.Parent.Name;

            Attributes = info.Attributes;

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
            } catch { }
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
