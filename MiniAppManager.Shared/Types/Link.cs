using System;

namespace Bluegrams.Application
{
    /// <summary>
    /// Represents a link item.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// The link url.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The link text to be displayed.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Creates a new Link item.
        /// </summary>
        /// <param name="url">The link url</param>
        public Link(string url) : this(url, url) { }

        /// <summary>
        /// Creates a new Link item.
        /// </summary>
        /// <param name="url">The link url</param>
        /// <param name="description">The link text to be displayed.</param>
        public Link(string url, string description)
        {
            Url = url;
            Description = description;
        }
    }
}
