using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions;
using Mm0205.IconConverter.Exceptions;

namespace Mm0205.IconConverter
{
    /// <summary>
    /// Converts an image to the image whose format is ".ico".
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Temporary folder.
        /// </summary>
        private const string TempFolderSubPath = "Mm0205\\IconConverter";

        /// <summary>
        /// The file system。
        /// </summary>
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fileSystem">
        /// The file system. <br/>
        /// If the parameter is <c>null</c>, this class uses the default file system.
        /// This parameter is only for unit tests.
        /// </param>
        public Converter(IFileSystem? fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public Image CreateIconImageFromFilePath(string sourceImageFilePath)
        {
            return CreateIconImageFromFilePath(sourceImageFilePath, ConversionParameter.DefaultParameter);
        }

        public Image CreateIconImageFromFilePath(string sourceImageFilePath, ConversionParameter parameter)
        {
            var image = LoadImage(sourceImageFilePath);
            return ConvertImageToIconBitmap(image, parameter);
        }

        private Image LoadImage(string sourceImageFilePath)
        {
            try
            {
                return new Bitmap(new MemoryStream(_fileSystem.File.ReadAllBytes(sourceImageFilePath)));
            }
            catch (Exception)
            {
                throw new ImageConversionException(ErrorKind.LoadImage, sourceImageFilePath);
            }
        }

        /// <summary>
        /// Convert an image to the image whose format is ".ico".
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <returns>Icon image.</returns>
        public Image ConvertImageToIconBitmap(Image image)
        {
            return ConvertImageToIconBitmap(image, ConversionParameter.DefaultParameter);
        }

        /// <summary>
        /// Convert an image to the image whose format is ".ico".
        /// </summary>
        /// <param name="image">Source image.</param>
        /// <param name="parameter">Conversion parameter. <see cref="ConversionParameter"/></param>
        /// <returns>Icon image.</returns>
        public Image ConvertImageToIconBitmap(Image image, ConversionParameter parameter)
        {
            var iconSize = ComputeIconSize(new Size(image.Width, image.Height), parameter);

            var edgeLength = Math.Max(iconSize.Width, iconSize.Height);

            using var bmp = new Bitmap(edgeLength, edgeLength);

            using (var g = Graphics.FromImage(bmp))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                var offsetX = (int)(Math.Round((bmp.Width - iconSize.Width) / 2.0));
                var offsetY = (int)(Math.Round((bmp.Height - iconSize.Height) / 2.0));
                g.DrawImage(image, new Rectangle(offsetX, offsetY, iconSize.Width, iconSize.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }

            var filePath = CreateTempFilePath();
            bmp.Save(filePath, ImageFormat.Icon);
            return new Bitmap(new MemoryStream(_fileSystem.File.ReadAllBytes(filePath)));
        
        }

        private string CreateTempFilePath()
        {
            var folderPath = CreateTempFolderPath();
            var fileName = Guid.NewGuid() + ".ico";
            return Path.Combine(folderPath, fileName);
        }

        private string CreateTempFolderPath()
        {
            return Path.Combine(_fileSystem.Path.GetTempPath(),
                TempFolderSubPath,
                DateTime.Now.ToString("yyyyMMddHHmmssfff") + Guid.NewGuid()
            );
        }

        /// <summary>
        /// Computes the result icon image size.
        /// </summary>
        /// <param name="originalSize">The original size.</param>
        /// <param name="parameter">The conversion parameter.</param>
        /// <returns>Icon image size.</returns>
        /// <seealso cref="ResizeKind"/>
        private Size ComputeIconSize(Size originalSize, ConversionParameter parameter)
        {
            // If the resize kind is None, do nothing.
            if (parameter.ResizeKind == ResizeKind.None)
            {
                return new Size(originalSize.Width, originalSize.Height);
            }

            // If the original image size is smaller then MaxSize, do noting.
            if (originalSize.Width <= parameter.MaxSize && originalSize.Height <= parameter.MaxSize)
            {
                return new Size(originalSize.Width, originalSize.Height);
            }

            var isMaximumSmaller = parameter.ResizeKind == ResizeKind.KeepAspect
                                   || parameter.ResizeKind == ResizeKind.MaximumSizeSmallerThanSquare;

            return Resize(originalSize, parameter.MaxSize, isMaximumSmaller);

        }

        /// <summary>
        /// Resizes the original size while keeping aspect ratio.
        /// </summary>
        /// <param name="originalSize">The original size.</param>
        /// <param name="parameterMaxSize">The max size.</param>
        /// <param name="isMaximumSmaller">
        /// If this is true, the both edges of result are smaller than or equal to <c>parameterMaxSize</c>
        /// Otherwise, the bot edges of result are larger then or equal to <c>parameterMaxSize</c>.
        /// </param>
        /// <returns></returns>
        private Size Resize(Size originalSize, int parameterMaxSize, bool isMaximumSmaller)
        {
            var widthIsLonger = originalSize.Width >= originalSize.Height;
            if (widthIsLonger && isMaximumSmaller || !widthIsLonger && !isMaximumSmaller)
            {
                var ratio = (double)parameterMaxSize / originalSize.Width;
                var height = (int)Math.Round(originalSize.Height * ratio);
                return new Size(parameterMaxSize, height);
            }
            else
            {
                var ratio = (double)parameterMaxSize / originalSize.Height;
                var width = (int)Math.Round(originalSize.Height * ratio);
                return new Size(width, parameterMaxSize);
            }
        }
    }

    /// <summary>
    /// Error Kinds.
    /// </summary>
    public enum ErrorKind
    {
        LoadImage
    }
}
