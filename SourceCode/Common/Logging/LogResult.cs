using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Logging
{
    public class LogResult
    {
        private bool _hasError = false;
        private string _errorMessage = string.Empty;
        private string _stackTrace = string.Empty;
        private string _operation = string.Empty;
        private Exception _ex = null;
		private string _simpleResult = string.Empty;

		public LogResult()
		{
			HasError = false;
		}

		public LogResult(Exception ex)
		{
			SetError(ex);
		}

        public string SimpleResult
        {
            get
            {
                return _simpleResult;
            }
            set
            {
                _simpleResult = value;
            }
        }

        public bool HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                _hasError = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
            }
        }

        public string StackTrace
        {
            get
            {
                return _stackTrace;
            }
            set
            {
                _stackTrace = value;
            }
        }

        public string Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }

        public Exception LastException
        {
            get
            {
                return _ex;
            }
        }

        public void SetError(Exception ex, string message = null)
        {
            message = (message != null) ? String.Format("{0}: {1}", message, ex.Message) : ex.Message;

            this._ex = ex;
            HasError = true;
            ErrorMessage = message;
            StackTrace = ex.StackTrace;
        }
    }
}
