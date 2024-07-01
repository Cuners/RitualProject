using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RitualProject
{
    public class FileExtensionToPackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fileURL)
            {
                string extension = Path.GetExtension(fileURL)?.ToLower();
                switch (extension)
                {
                    case ".doc":
                    case ".docx":
                        return PackIconKind.MicrosoftWord;
                    case ".xls":
                    case ".xlsx":
                        return PackIconKind.MicrosoftExcel;
                    case ".mp4":
                        return PackIconKind.Video;
                    case ".pdf":
                        return PackIconKind.FilePdfBox;
                    // Добавьте другие расширения и соответствующие значки при необходимости
                    default:
                        return PackIconKind.File;
                }
            }

            return PackIconKind.File;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
