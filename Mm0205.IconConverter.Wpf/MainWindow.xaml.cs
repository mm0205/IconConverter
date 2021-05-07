using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using File = System.IO.File;

namespace Mm0205.IconConverter.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 実行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExecute_Click(object sender, RoutedEventArgs e)
        {
            var path = TextBoxPath.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("対象のファイル/フォルダパスを指定してください。", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxPath.Focus();
                return;
            }

            var targets = new List<string>();

            if (File.Exists(TextBoxPath.Text))
            {
                targets.Add(TextBoxPath.Text);
            }
            else if (Directory.Exists(TextBoxPath.Text))
            {
                var availableExtensions = new List<string>()
                {
                    ".png",
                    ".jpg",
                    ".jpeg",
                };

                var files = Directory.GetFiles(TextBoxPath.Text);
                targets.AddRange(files.Where(x => availableExtensions.Contains(Path.GetExtension(x))));
            }

            var iconConverter = new Converter();
            foreach (var filePath in targets)
            {
                var icon = iconConverter.CreateIconImageFromFilePath(filePath);
                
                icon.Save(Path.ChangeExtension(filePath, ".conv.ico"),ImageFormat.Icon);

            }
        }


        private void ButtonFileReference_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new CommonOpenFileDialog();
            ofd.Filters.Add(new CommonFileDialogFilter("画像ファイル", "*.jpg;*.jpeg;*.png;*.bmp"));
            if (ofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            TextBoxPath.Text = ofd.FileName;
        }

        private void ButtonFolderReference_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new CommonOpenFileDialog();
            ofd.IsFolderPicker = true;
            if (ofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            TextBoxPath.Text = ofd.FileName;
        }
    }
}
