using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Common.Interfaces
{
    interface IDataAccess
    {
        DataOperationResult GetFileData();
        DataOperationResult GetDirectoryData();

        DataOperationResult InsertFileData(FileData data);
        DataOperationResult InsertDirectoryData(DirectoryData data);

        DataOperationResult InsertFileData(IEnumerable<FileData> data);
        DataOperationResult InsertDirectoryData(IEnumerable<DirectoryData> data);

        DataOperationResult UpdateFileData(FileData data);
        DataOperationResult UpdateDirectoryData(DirectoryData data);

        DataOperationResult DeleteFileData(FileData data);
        DataOperationResult DeleteDirectoryData(DirectoryData data);
    }
}
