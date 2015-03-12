using System;
using System.Collections;

namespace Common.Objects
{
    /// <summary>
    /// Summary description for OperationResult
    /// </summary>
    public class OperationResult
    {
        private Guid guid;
        private Hashtable results = new Hashtable(StringComparer.OrdinalIgnoreCase);
        private Exception exception;
        private TimeSpan elapsedTime;
        private DateTime start;
        private DateTime end;

        public OperationResult()
        {
            guid = Guid.NewGuid();
        }

        public virtual string GetDebugMessage()
        {
            return null;
        }

        public object Result
        {
            get
            {
                if (results.ContainsKey(guid.ToString()))
                {
                    return GetResult(guid.ToString());
                }
                return null;
            }
            set
            {
                UpdateResult(guid.ToString(), value);
            }
        }

        public void UpdateResult(string key, object value)
        {
            if (results.ContainsKey(key) == false)
            {
                results.Add(key, value);
            }
            else
            {
                results[key] = value;
            }
        }
        public void AddResult(string key, object value)
        {
            if (results.ContainsKey(key) == false)
            {
                results.Add(key, value);
            }
        }
        public object GetResult(string key)
        {
            if (results.ContainsKey(key))
            {
                return results[key];
            }
            return null;
        }

        public void AddException(Exception ex)
        {
            exception = ex;
        }

        public void Start()
        {
            start = DateTime.Now;
        }

        public void End()
        {
            end = DateTime.Now;
            elapsedTime = end - start;
        }

        public bool IsValid
        {
            get
            {
                return exception == null;
            }
        }

        public Exception GetException
        {
            get
            {
                return exception;
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
        }
    }
}
