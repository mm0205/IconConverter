using System;

namespace Mm0205.IconConverter.Exceptions
{
    /// <summary>
    /// The root exception used in this library.
    /// </summary>
    public class ImageConversionException : Exception
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message">Error message.</param>
        public ImageConversionException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kind">The error kind.</param>
        /// <param name="additional">Additional info.</param>
        public ImageConversionException(ErrorKind kind, object additional)
            : base($"Image Conversion Error kind={kind} {additional}")
        {
        }
    }
}
