namespace Mm0205.IconConverter
{
    /// <summary>
    /// Resize algorithms..
    /// </summary>
    public enum ResizeKind
    {
        /// <summary>
        /// Doesn't resize the image.
        /// <br/>
        /// The resulting image will have the same size as original image..
        /// </summary>
        None,

        /// <summary>
        /// If the long side's length is longer then <see cref="ConversionParameter.MaxSize"/>,
        /// then the long side's will be <c>MaxSize</c>.
        /// <br/>
        /// The resulting image will have the same aspect ratio as original image.
        /// </summary>
        KeepAspect,

        /// <summary>
        ///The resulting image will be a square with a transparent background. <br/>
        /// If the long side's length of the image is less than or equal to <see cref="ConversionParameter.MaxSize"/>,
        /// the original image will be pasted in the center of the square.<br/>
        /// Otherwise,the original image will be resized so that its longest side is MaxSize,
        /// while keeping its aspect ratio and pasted into the center of the square.
        /// </summary>
        MaximumSizeSmallerThanSquare,

        /// <summary>
        /// If the edges of the original image are smaller than <see cref="ConversionParameter.MaxSize"/>,
        /// the resulting image will be the same as for MaximumSizeSmallerThanSquare.<br/>
        /// <br/>
        /// Otherwise, the original image will be resized to the minimum size
        /// where both edges are greater than or equal to [MaxSize]
        /// while keeping the aspect ratio.
        /// </summary>
        MinimumSizeLargerThanSquare
    }
}