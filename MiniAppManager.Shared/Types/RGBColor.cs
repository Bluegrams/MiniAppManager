using System;

namespace Bluegrams.Application
{
    /// <summary>
    /// Represents an RGB color that can be converted to System.Drawing.Color and System.Windows.Media.Color.
    /// </summary>
    public class RGBColor
    {
        /// <summary>
        /// The RGB red value.
        /// </summary>
        public byte R { get; private set; }
        /// <summary>
        /// The RGB green value.
        /// </summary>
        public byte G { get; private set; }
        /// <summary>
        /// The RGB blue value.
        /// </summary>
        public byte B { get; private set; }

        public RGBColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Converts an RGBColor to a System.Drawing color.
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        public static explicit operator System.Drawing.Color(RGBColor color)
        {
            return System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts an RGBColor to a System.Windows.Media color.
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        public static explicit operator System.Windows.Media.Color(RGBColor color)
        {
            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B);
        }
    }
}
