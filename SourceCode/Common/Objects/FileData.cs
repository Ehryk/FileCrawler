﻿using System;
using System.IO;
using System.Data;
using Common.Extensions;
using SevenZip;

namespace Common.Objects
{
    public class FileData
    {
        #region Properties

        public int? ID;
        public string Path;

        public string Root;
        public bool IsNetwork;
        public bool IsLocal;

        public string Directory;
        public string ParentName;
        public string Name;
        public string Extension;
        public bool ReadOnly;
        public FileAttributes Attributes;
        public bool Hidden { get { return Attributes.HasAttribute(FileAttributes.Hidden); } }

        public bool IsCompressedContainer;
        public bool IsContained;
        public string ContainerPath;
        public string ContainerName;
        public string ContainedDirectory;
        public string ContainedPath;

        public long Size;

        public decimal KB { get { return Size.GetKB(); } }
        public decimal MB { get { return Size.GetMB(); } }
        public decimal GB { get { return Size.GetGB(); } }

        public DateTime? CreateTime;
        public DateTime? CreateTimeUtc;
        public DateTime? LastAccessTime;
        public DateTime? LastAccessTimeUtc;
        public DateTime? LastWriteTime;
        public DateTime? LastWriteTimeUtc;

        #endregion

        #region Constructors

        public FileData(string path)
        {
            Path = path;

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            Name = System.IO.Path.GetFileName(path);
            Extension = System.IO.Path.GetExtension(path);
            Directory = System.IO.Path.GetDirectoryName(path);
            ParentName = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(path));
        }

        public FileData(FileInfo info)
        {
            if (!info.Exists)
                throw new FileNotFoundException(String.Format("File not found: {0}", info.FullName));

            Path = info.FullName;

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            Directory = info.DirectoryName;
            if (info.Directory != null && info.Directory.Parent != null)
                ParentName = info.Directory.Parent.Name;
            Name = info.Name;
            Extension = info.Extension;
            ReadOnly = info.IsReadOnly;

            Size = info.Length;

            Attributes = info.Attributes;

            try
            {
                CreateTime = info.CreationTime;
                CreateTimeUtc = info.CreationTimeUtc;
            }
            catch { }
            try
            {
                LastAccessTime = info.LastAccessTime;
                LastAccessTimeUtc = info.LastAccessTimeUtc;
            }
            catch { }
            try
            {
                LastWriteTime = info.LastWriteTime;
                LastWriteTimeUtc = info.LastWriteTimeUtc;
            }
            catch { }
        }

        public FileData(ArchiveFileInfo info, FileData container)
        {
            Path = String.Format(@"{0}/{1}", container.Path, info.FileName.Replace("\\", "/"));

            Root = System.IO.Path.GetPathRoot(Path);
            //Are there other ways a non-UNC path can be non-local?
            IsNetwork = new Uri(Path).IsUnc;
            IsLocal = !IsNetwork;

            Directory = container.Directory;
            ParentName = container.ParentName;

            IsContained = true;
            ContainerPath = container.Path;
            ContainerName = container.Name;
            ContainedDirectory = System.IO.Path.GetDirectoryName(info.FileName);
            ContainedPath = info.FileName.Replace("\\", "/");
            ReadOnly = container.ReadOnly;

            Name = System.IO.Path.GetFileName(info.FileName);
            Extension = System.IO.Path.GetExtension(info.FileName);

            Size = (long)info.Size;

            //How can FileAttributes be retrived from uint?
            Attributes = (FileAttributes)info.Attributes;

            try
            {
                CreateTime = info.CreationTime;
                CreateTimeUtc = info.CreationTime.ToUniversalTime();
            }
            catch { }
            try
            {
                LastAccessTime = info.LastAccessTime;
                LastAccessTimeUtc = info.LastAccessTime.ToUniversalTime();
            }
            catch { }
            try
            {
                LastWriteTime = info.LastWriteTime;
                LastWriteTimeUtc = info.LastWriteTime.ToUniversalTime();
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
        
        #endregion
    }
}
