using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Extensions;

namespace FileCrawler
{
    class AppSettings
    {
        #region Private Key Definitions

        private static string leaveConsoleOpen = LoadValue("LeaveConsoleOpen");
        private static string processingTemp = LoadValue("ProcessingTemp");

        private static ConnectionStringSettings connection_Main = LoadConnection("Main");

        #endregion

        #region Public Object Accessors

        public static bool LeaveConsoleOpen
        {
            get { return leaveConsoleOpen.ToBoolean(); }
        }

        public static string ProcessingTemp
        {
            get { return processingTemp; }
        }

        public static ConnectionStringSettings Connection_Main
        {
            get { return connection_Main; }
        }

        public static string ConnectionProvider_Main
        {
            get { return connection_Main.ProviderName; }
        }

        public static string ConnectionString_Main
        {
            get { return connection_Main.ConnectionString; }
        }

        #endregion

        #region Methods

        public static string LoadValue(string pKey, bool pRequired = false)
        {
            if (ConfigurationManager.AppSettings[pKey] == null)
            {
                if (pRequired)
                    throw new Exception(String.Format("Required Configuration Setting not found: {0}", pKey));

                return null;
            }

            return ConfigurationManager.AppSettings[pKey];
        }

        public static ConnectionStringSettings LoadConnection(string pName, bool pRequired = false)
        {
            if (ConfigurationManager.ConnectionStrings[pName] == null)
            {
                if (pRequired)
                    throw new Exception(String.Format("Required Configuration Setting not found: {0}", pName));

                return null;
            }

            return ConfigurationManager.ConnectionStrings[pName];
        }

        #endregion
    }
}
