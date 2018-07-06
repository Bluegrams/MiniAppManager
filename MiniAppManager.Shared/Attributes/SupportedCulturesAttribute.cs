using System;
using System.Globalization;

namespace Bluegrams.Application.Attributes
{
    /// <summary>
    /// Explicitly specifies cultures that are supported by this assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportedCulturesAttribute : Attribute
    {
        /// <summary>
        /// An array of all explicitly supported cultures.
        /// </summary>
        public CultureInfo[] SupportedCultures { get; private set; }

        public SupportedCulturesAttribute(params string[] cultures)
        {
            SupportedCultures = new CultureInfo[cultures.Length];
            for (int i = 0; i < cultures.Length; i++)
            {
                SupportedCultures[i] = new CultureInfo(cultures[i]);
            }
        }
    }
}
