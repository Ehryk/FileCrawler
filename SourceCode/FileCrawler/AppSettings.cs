using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;

namespace FileCrawler
{
    class AppSettings
    {
        #region Private Key Definitions

        private static string leaveConsoleOpen = LoadValue("LeaveConsoleOpen");
        private static string processingTemp = LoadValue("ProcessingTemp");
        private static string reportDirectories = LoadValue("ReportDirectories");

        private static string compressed_PathTo7zDLL = LoadValue("Compressed_PathTo7z.dll");
        private static string compressed_ReportContainers = LoadValue("Compressed_ReportContainers");
        private static string compressed_Extensions = LoadValue("Compressed_Extensions");
        private static List<string> compressed_Extension_List;
        private static string compressed_ReadContents = LoadValue("Compressed_ReadContents");
        private static string compressed_ReadContents_Recurse = LoadValue("Compressed_ReadContents_Recurse");

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

        public static bool ReportDirectories
        {
            get { return reportDirectories.ToBoolean(); }
        }

        public static string Compressed_PathTo7zDLL
        {
            get { return compressed_PathTo7zDLL; }
        }

        public static bool Compressed_ReportContainers
        {
            get { return compressed_ReportContainers.ToBoolean(); }
        }

        public static List<string> Compressed_Extensions
        {
            get 
            {
                if (compressed_Extension_List == null)
                    compressed_Extension_List = LoadList(compressed_Extensions.Replace(".", ""));

                return compressed_Extension_List;
            }
        }

        public static bool Compressed_ReadContents
        {
            get { return compressed_ReadContents.ToBoolean(); }
        }

        public static bool Compressed_ReadContents_Recurse
        {
            get { return compressed_ReadContents_Recurse.ToBoolean(); }
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

        public static List<string> LoadList(string pValue, char pSeparator = ',')
        {
            if (pValue == null)
                return new List<string>();

            return pValue.Split(pSeparator).ToList();
        }

        #endregion
    }
}
