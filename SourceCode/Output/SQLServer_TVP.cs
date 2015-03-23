using System;
using System.Collections.Generic;
using System.Data;
using Common.Events;
using Common.Interfaces;
using System.IO;
using Common.Objects;
using Microsoft.SqlServer.Server;

namespace Output
{
    public class SQLServer_TVP : IOutput
    {
        #region Properties

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

        public bool UsesCrawlStart() { return true; }
        public bool UsesDirectoryFound() { return false; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return true; }
        public bool UsesDirectoryProcessed() { return true; }
        public bool UsesCrawlError() { return false; }
        public bool UsesFileInaccessible() { return false; }
        public bool UsesDirectoryInaccessible() { return false; }
        public bool UsesCrawlEnd() { return true; }

        public void CrawlStart(object sender, EventArgs e)
        {
            //Clear Loading
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            if (e.FileData.IsCompressedContainer)
            {
                //Insert Container's Files
            }
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
            //Insert Directory's Files
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            //Perform Diff
        }

        public bool AttachToCrawler(ICrawler pCrawler)
        {
            //if (UsesCrawlStart())
            //   pCrawler.
            return true;
        }

        #endregion

        #region Methods

        private static DataTable CreateDataTable(IEnumerable<FileData> pFiles)
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int?));
            table.Columns.Add("Path", typeof(string));
            table.Columns.Add("IsCompressedContainer", typeof(bool));
            table.Columns.Add("IsContained", typeof(bool));
            table.Columns.Add("Size", typeof(long));
            table.Columns.Add("LastWriteTime", typeof(DateTime?));
            foreach (var file in pFiles)
            {
                table.Rows.Add(file.ID, file.Path, file.IsCompressedContainer, file.IsContained, file.Size, file.LastWriteTime);
            }
            return table;
        }

        private static IEnumerable<SqlDataRecord> CreateSqlDataRecords(IEnumerable<FileData> pFiles)
        {
            SqlMetaData[] metaData = new SqlMetaData[6];
            metaData[0] = new SqlMetaData("ID", SqlDbType.Int);
            metaData[1] = new SqlMetaData("Path", SqlDbType.NVarChar);
            metaData[2] = new SqlMetaData("IsCompressedContainer", SqlDbType.Bit);
            metaData[3] = new SqlMetaData("IsContained", SqlDbType.Bit);
            metaData[4] = new SqlMetaData("Size", SqlDbType.BigInt);
            metaData[5] = new SqlMetaData("LastWriteTime", SqlDbType.DateTime2);
            SqlDataRecord record = new SqlDataRecord(metaData);
            foreach (var file in pFiles)
            {
                record.SetNullableInt32(0, file.ID);
                record.SetString(1, file.Path);
                record.SetBoolean(2, file.IsCompressedContainer);
                record.SetBoolean(3, file.IsContained);
                record.SetInt64(4, file.Size);
                record.SetNullableDateTime(5, file.LastWriteTime);
                yield return record;
            }
        }

        #endregion
    }

    public static class SqlDataRecordExtensions
    {
        public static void SetNullableBoolean(this SqlDataRecord rec, int index, bool? value)
        {
            if (value.HasValue)
                rec.SetBoolean(index, value.GetValueOrDefault());
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableInt32(this SqlDataRecord rec, int index, Int32? value)
        {
            if (value.HasValue)
                rec.SetInt32(index, value.GetValueOrDefault());
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableInt64(this SqlDataRecord rec, int index, Int64? value)
        {
            if (value.HasValue)
                rec.SetInt64(index, value.GetValueOrDefault());
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableDateTime(this SqlDataRecord rec, int index, DateTime? value)
        {
            if (value.HasValue)
                rec.SetDateTime(index, value.GetValueOrDefault());
            else
                rec.SetDBNull(index);
        }
    }
}
