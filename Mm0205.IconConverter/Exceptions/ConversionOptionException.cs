namespace Mm0205.IconConverter.Exceptions
{
    /// <summary>
    /// The exception for image conversion options.
    /// </summary>
    public class ConversionOptionException : ImageConversionException
    {
        /// <summary>
        /// 
        /// </summary>
        public ConversionOptionKind OptionKind { get; } = ConversionOptionKind.None;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="kind">The kind of conversion option.</param>
        /// <param name="invalidValue">The value that caused the exception.</param>
        public ConversionOptionException(ConversionOptionKind kind, object invalidValue)
        : base($"invalid option: {kind} = {invalidValue}")
        {
            OptionKind = kind;
        }
    }

    public enum ConversionOptionKind
    {
        None,

        MaxSize,
        ResizeKind
    }
}