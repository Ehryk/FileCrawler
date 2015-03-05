using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Extensions
{
    static public class AssemblyInfo
    {
        public static Version Version { get { return Assembly.GetCallingAssembly().GetName().Version; } }

        public static string Title
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title.Length > 0) return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string ProductName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string Description
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public static string CopyrightHolder
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public static string CompanyName
        {
            get
            {
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public static string Path
        {
            get
            {
                return Assembly.GetCallingAssembly().Location;
            }
        }

        public static string DirectoryName
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }
        }

        public static string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(Assembly.GetCallingAssembly().Location);
            }
        }

        public static string Name
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Name;
            }
        }

        public static ProcessorArchitecture Architecture
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().ProcessorArchitecture;
            }
        }
    }
}
