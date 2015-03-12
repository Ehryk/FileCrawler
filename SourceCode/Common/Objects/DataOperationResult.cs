using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Common.Objects
{
    /// <summary>
    /// Summary description for DataOperationResult
    /// </summary>
    public class DataOperationResult : OperationResult
    {
        private string _connection;

        private Hashtable _resultParams = new Hashtable();
        private Hashtable _information = new Hashtable();
        private Dictionary<string, string> _inputParams = new Dictionary<string, string>();

        public DataOperationResult()
        {
            Rows = -1;
        }

        public DataOperationResult(string connection)
        {
            Rows = -1;
            _connection = connection;
        }

        public DataOperationResult(string connection, string sql)
        {
            Rows = -1;
            _connection = connection;
            Sql = sql;
        }

        public override string GetDebugMessage()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in InputParams.Keys)
            {
                if (sb.Length > 0)
                    sb.Append(",");
                sb.AppendFormat("{0}:{1}", key, InputParams[key]);
            }

            string result = (IsValid) ? "Success" : "Failure";

            string debug = string.Format("SQL:{0} Params:{1} Result:{2}", Sql, sb, result);

            return debug;
        }

        public void AddInputParam(string key, object value)
        {
            _inputParams.Add(key, value.ToString());
        }

        public Dictionary<string, string> InputParams
        {
            get
            {
                return _inputParams;
            }
        }

        public string Connection
        {
            get
            {
                return _connection;
            }
        }

        public string Sql { get; set; }

        public int Rows { get; set; }

        public void AddInfo(string key, object value)
        {
            if (_information.ContainsKey(key) == false)
            {
                _information.Add(key, value);
            }
        }

        public object GetInfo(string key)
        {
            return _information[key];
        }

        public void AddResultParam(string key, string value)
        {
            if (_resultParams.ContainsKey(key) == false)
            {
                _resultParams.Add(key, value);
            }
        }

        public object GetResultParam(string key)
        {
            return _resultParams[key];
        }

        public DataSet DataSetResult
        {
            get
            {
                return (DataSet)Result;
            }
            set
            {
                Result = value;
            }
        }

        public DataTable GetDataTable(int tableIndex)
        {
            if (DataSetResult != null && DataSetResult.Tables.Count > tableIndex)
                return DataSetResult.Tables[tableIndex];
            return null;
        }

        public string GetColumnResult(int table, int row, string column)
        {
            string result = null;

            Type type = Result.GetType();
            switch (type.Name)
            {
                case "DataTable":
                    break;
                case "DataSet":
                    DataSet ds = (DataSet)Result;
                    if (ds != null && ds.Tables.Count > table)
                    {
                        DataTable dt = ds.Tables[table];
                        if (dt != null && dt.Rows.Count > row)
                        {
                            DataRow dr = dt.Rows[row];
                            if (dr != null)
                            {
                                if (dr[column] != null)
                                {
                                    result = dr[column].ToString();
                                }
                            }
                        }
                    }
                    break;
                case "Int32":
                    result = Result.ToString();
                    break;
                default:
                    result = Result.ToString();
                    break;
            }

            return result;
        }

        public string GetColumnResult(string column)
        {
            return GetColumnResult(0, 0, column);
        }
    }

}
