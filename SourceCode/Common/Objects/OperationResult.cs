using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Common
{
    /// <summary>
    /// Summary description for OperationResult
    /// </summary>
    public class OperationResult
    {
        private Guid _guid;
        private Hashtable _results = new Hashtable(StringComparer.OrdinalIgnoreCase);
        private Exception _exception;
        private TimeSpan _elapsedTime;
        private DateTime _start;
        private DateTime _end;

        public OperationResult()
        {
            _guid = Guid.NewGuid();
        }

        public virtual string GetDebugMessage()
        {
            return null;
        }

        public object Result
        {
            get
            {
                if (_results.ContainsKey(_guid.ToString()))
                {
                    return GetResult(_guid.ToString());
                }
                return null;
            }
            set
            {
                UpdateResult(_guid.ToString(), value);
            }
        }

        public void UpdateResult(string key, object value)
        {
            if (_results.ContainsKey(key) == false)
            {
                _results.Add(key, value);
            }
            else
            {
                _results[key] = value;
            }
        }
        public void AddResult(string key, object value)
        {
            if (_results.ContainsKey(key) == false)
            {
                _results.Add(key, value);
            }
        }
        public object GetResult(string key)
        {
            if (_results.ContainsKey(key))
            {
                return _results[key];
            }
            return null;
        }

        public void AddException(Exception ex)
        {
            _exception = ex;
        }

        public void Start()
        {
            _start = DateTime.Now;
        }

        public void End()
        {
            _end = DateTime.Now;
            _elapsedTime = (TimeSpan)(_end - _start);
        }

        public bool IsValid
        {
            get
            {
                return _exception == null;
            }
        }

        public Exception GetException
        {
            get
            {
                return _exception;
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                return _elapsedTime;
            }
        }
    }
}
