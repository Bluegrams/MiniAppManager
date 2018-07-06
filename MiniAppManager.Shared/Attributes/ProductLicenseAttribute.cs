using System;

namespace Bluegrams.Application.Attributes
{
    /// <summary>
    /// Specifies the license under which the product is published.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ProductLicenseAttribute : Attribute
    {
        /// <summary>
        /// The link to the product license.
        /// </summary>
        public Link LicenseLink { get; private set; }

        public ProductLicenseAttribute(string url)
        {
            LicenseLink = new Link(url);
        }

        public ProductLicenseAttribute(string url, string displayText)
        {
            LicenseLink = new Link(url, displayText);
        }
    }
}
