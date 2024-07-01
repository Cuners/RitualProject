using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace RitualProject
{
    public class PackIconVideoPreview : PackIcon
    {
        public static readonly DependencyProperty PreviewSourceProperty =
            DependencyProperty.Register("PreviewSource", typeof(ImageSource), typeof(PackIconVideoPreview));

        public ImageSource PreviewSource
        {
            get { return (ImageSource)GetValue(PreviewSourceProperty); }
            set { SetValue(PreviewSourceProperty, value); }
        }

        static PackIconVideoPreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconVideoPreview), new FrameworkPropertyMetadata(typeof(PackIconVideoPreview)));
        }
    }
}
