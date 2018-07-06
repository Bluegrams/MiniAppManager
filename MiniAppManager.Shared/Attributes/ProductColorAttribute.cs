using System;

namespace Bluegrams.Application.Attributes
{
    /// <summary>
    /// Specifies the color used for the 'About' box of the product.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ProductColorAttribute : Attribute
    {
        /// <summary>
        /// The product color as RGBColor.
        /// </summary>
        public RGBColor ProductColor { get; private set; }

        public ProductColorAttribute(byte r, byte g, byte b)
        {
            ProductColor = new RGBColor(r, g, b);
        }
    }
}
