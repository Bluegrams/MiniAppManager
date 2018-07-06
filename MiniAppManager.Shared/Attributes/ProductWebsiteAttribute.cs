using System;

namespace Bluegrams.Application.Attributes
{
    /// <summary>
    /// Specifies the product website of the assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ProductWebsiteAttribute : Attribute
    {
        /// <summary>
        /// The link to the product website.
        /// </summary>
        public Link WebsiteLink { get; private set; }

        public ProductWebsiteAttribute(string url)
        {
            WebsiteLink = new Link(url);
        }

        public ProductWebsiteAttribute(string url, string displayText)
        {
            WebsiteLink = new Link(url, displayText);
        }
    }
}
