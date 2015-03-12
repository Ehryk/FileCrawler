using System;

namespace Common.Objects
{
    public class LogResult : OperationResult
    {
        private string errorMessage = String.Empty;
        private string stackTrace = String.Empty;
        private string operation = String.Empty;
        private Exception ex;
        private string simpleResult = String.Empty;

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
                return simpleResult;
            }
            set
            {
                simpleResult = value;
            }
        }

        public bool HasError { get; set; }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }

        public string StackTrace
        {
            get
            {
                return stackTrace;
            }
            set
            {
                stackTrace = value;
            }
        }

        public string Operation
        {
            get
            {
                return operation;
            }
            set
            {
                operation = value;
            }
        }

        public Exception LastException
        {
            get
            {
                return ex;
            }
        }

        public void SetError(Exception ex, string message = null)
        {
            message = (message != null) ? String.Format("{0}: {1}", message, ex.Message) : ex.Message;

            this.ex = ex;
            HasError = true;
            ErrorMessage = message;
            StackTrace = ex.StackTrace;
        }
    }
}
