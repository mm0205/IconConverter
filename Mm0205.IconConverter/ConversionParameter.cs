using Mm0205.IconConverter.Exceptions;

namespace Mm0205.IconConverter
{
    /// <summary>
    /// Options for image conversion.
    /// </summary>
    public class ConversionParameter
    {
        private const int MinimumMaxLength = 1;

        private const int MaximumMaxLength = 500;

        /// <summary>
        /// Resizing algorithm. <br/>
        /// Default is <see cref="Mm0205.IconConverter.ResizeKind.None"/>.
        /// </summary>
        /// <exception cref="ConversionOptionException">Invalid value is passed.</exception>
        public ResizeKind ResizeKind
        {
            get => _resizeKind;
            set
            {
                if (value < ResizeKind.None)
                {
                    throw new ConversionOptionException(ConversionOptionKind.ResizeKind, value);
                }

                if (value > ResizeKind.MinimumSizeLargerThanSquare)
                {
                    throw new ConversionOptionException(ConversionOptionKind.ResizeKind, value);
                }

                _resizeKind = value;
            }
        }

        private ResizeKind _resizeKind = ResizeKind.None;

        /// <summary>
        /// The maximum length of the edge of a square.
        /// <br/>
        /// Default is 250.
        /// </summary>
        /// <exception cref="ConversionOptionException">Invalid value is passed.</exception>
        public int MaxSize
        {
            get => _maxSize;
            set
            {
                if (value < MinimumMaxLength)
                {
                    throw new ConversionOptionException(ConversionOptionKind.MaxSize, value);
                }

                if (value > MaximumMaxLength)
                {
                    throw new ConversionOptionException(ConversionOptionKind.MaxSize, value);
                }

                _maxSize = value;
            }
        }

        private int _maxSize = 250;

        /// <summary>
        /// The default parameter.
        /// </summary>
        public static ConversionParameter DefaultParameter { get; } = new();
    }
}