using System;
using System.Configuration;
using System.Xml;
using Common.Objects;
using log4net;
using Common.Enums;
using Common.Extensions;

namespace Common.Logging
{
    public static class Logger
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string connectionString = ConfigurationManager.ConnectionStrings["Logging"] != null ? ConfigurationManager.ConnectionStrings["Logging"].ConnectionString : null;
        private static string logProcedure = ConfigurationManager.AppSettings["DataProcedure_LogInsert"];

        public static LogResult LogFatal(Exception pException, string pMessage = null, params object[] pFormatArguments)
        {
            string message = (pMessage != null) ? String.Format("{0}: {1}", String.Format(pMessage, pFormatArguments), pException.Message) : pException.Message;

            return LogItem(LogLevel.Fatal, message, pException.StackTrace);
        }

        public static LogResult LogFatal(string pMessage, string pStackTrace = null, params object[] pFormatArguments)
        {
            return LogItem(LogLevel.Fatal, String.Format(pMessage, pFormatArguments), pStackTrace);
        }

        public static LogResult LogError(Exception pException, string pMessage = null, params object[] pFormatArguments)
        {
            string message = (pMessage != null) ? String.Format("{0}: {1}", String.Format(pMessage, pFormatArguments), pException.Message) : pException.Message;

            return LogItem(LogLevel.Error, message, pException.StackTrace);
        }

        public static LogResult LogError(string pMessage, string pStackTrace = null, params object[] pFormatArguments)
        {
            return LogItem(LogLevel.Error, String.Format(pMessage, pFormatArguments), pStackTrace);
        }

        public static LogResult LogWarning(Exception pException, string pMessage = null, params object[] pFormatArguments)
        {
            string message = (pMessage != null) ? String.Format("{0}: {1}", String.Format(pMessage, pFormatArguments), pException.Message) : pException.Message;

            return LogItem(LogLevel.Warning, message, pException.StackTrace);
        }

        public static LogResult LogWarning(string pMessage, string pStackTrace = null, params object[] pFormatArguments)
        {
            return LogItem(LogLevel.Warning, String.Format(pMessage, pFormatArguments), pStackTrace);
        }

        public static LogResult LogInfo(string pMessage, params object[] pFormatArguments)
        {
            return LogItem(LogLevel.Info, String.Format(pMessage, pFormatArguments));
        }

        public static LogResult LogDebug(string pMessage, params object[] pFormatArguments)
        {
            return LogItem(LogLevel.Debug, String.Format(pMessage, pFormatArguments));
        }

        private static LogResult LogItem(LogLevel pLevel, string pMessage, string pStackTrace = null)
        {
            var lr = new LogResult();

            try
            {
                // Only configure log4net once per process
                if (!LogManager.GetRepository().Configured)
                {
                    ConfigurationSection logSection = ConfigurationManager.GetSection("log4net") as ConfigurationSection;
                    XmlElement logElement = logSection.SectionInformation.GetRawXml().ToXmlElement();
                    log4net.Config.XmlConfigurator.Configure(logElement);

                    if (_log.IsDebugEnabled)
                    {
                        _log.DebugFormat("Loaded log4net config in {0}.", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                lr.SetError(ex, String.Format("Could not configure log4net from {0}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.FullName));
            }

            try
            {
                var hier = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
                var adoAppender = hier.Root.GetAppender("AdoNetAppender") as log4net.Appender.AdoNetAppender;
                adoAppender.ConnectionString = connectionString;
                adoAppender.CommandText = logProcedure;
                adoAppender.ActivateOptions();

                string applicationName = AppDomain.CurrentDomain.FriendlyName.Split('.')[0];
                GlobalContext.Properties["application"] = applicationName;

                switch (pLevel)
                {
                    case LogLevel.Fatal:
                        GlobalContext.Properties["stack"] = pStackTrace;
                        _log.Fatal(pMessage);
                        break;
                    case LogLevel.Error:
                        GlobalContext.Properties["stack"] = pStackTrace;
                        _log.Error(pMessage);
                        break;
                    case LogLevel.Warning:
                        GlobalContext.Properties["stack"] = pStackTrace;
                        _log.Warn(pMessage);
                        break;
                    case LogLevel.Info:
                        _log.Info(pMessage);
                        break;
                    case LogLevel.Debug:
                        _log.Debug(pMessage);
                        break;
                    default:
                        _log.Info(pMessage);
                        break;
                }
            }
            catch (Exception ex)
            {
                lr.SetError(ex);
            }
            return lr;
        }
    }
}
