using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bluegrams.Application
{
    /// <summary>
    /// A class providing assembly information.
    /// </summary>
    public static class AppInfo
    {
        /// <summary>
        /// The product name of the assembly.
        /// </summary>
        public static string ProductName
        {
            get
            {
                return ((AssemblyProductAttribute)Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyProductAttribute))).Product;
            }
        }

        /// <summary>
        /// The title of the assembly.
        /// </summary>
        public static string Title
        {
            get
            {
                return ((AssemblyTitleAttribute)Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title;
            }
        }

        /// <summary>
        /// The company that published the assembly.
        /// </summary>
        public static string Company
        {
            get
            {
                return ((AssemblyCompanyAttribute)Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyCompanyAttribute))).Company;
            }
        }

        /// <summary>
        /// The assembly version.
        /// </summary>
        public static string Version
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// The copyright notice.
        /// </summary>
        public static string Copyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            }
        }

        /// <summary>
        /// The assembly description.
        /// </summary>
        public static string Description
        {
            get
            {
                return ((AssemblyDescriptionAttribute)Assembly.GetEntryAssembly().GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;
            }
        }
    }
}
