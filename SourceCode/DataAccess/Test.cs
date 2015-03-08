using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;

namespace DataAccess
{
    public class Test : IDataAccess
    {
        public DataOperationResult GetFileData()
        {
            return new DataOperationResult();
        }

        public DataOperationResult GetDirectoryData()
        {
            return new DataOperationResult();
        }


        public DataOperationResult InsertFileData(FileData data)
        {
            return new DataOperationResult();
        }

        public DataOperationResult InsertDirectoryData(DirectoryData data)
        {
            return new DataOperationResult();
        }


        public DataOperationResult InsertFileData(IEnumerable<FileData> data)
        {
            return new DataOperationResult();
        }

        public DataOperationResult InsertDirectoryData(IEnumerable<DirectoryData> data)
        {
            return new DataOperationResult();
        }


        public DataOperationResult UpdateFileData(FileData data)
        {
            return new DataOperationResult();
        }

        public DataOperationResult UpdateDirectoryData(DirectoryData data)
        {
            return new DataOperationResult();
        }


        public DataOperationResult DeleteFileData(FileData data)
        {
            return new DataOperationResult();
        }

        public DataOperationResult DeleteDirectoryData(DirectoryData data)
        {
            return new DataOperationResult();
        }
    }
}
