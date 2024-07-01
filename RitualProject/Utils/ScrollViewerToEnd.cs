using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace RitualProject
{
    public class ScrollViewerToEnd
    {
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ScrollViewerToEnd),
            new PropertyMetadata(false, AutoScrollPropertyChanged));

        private static void AutoScrollPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer != null && (bool)e.NewValue)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                scrollViewer.ScrollToEnd();
            }
            else
            {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            }
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0)
            {
                var scrollView = sender as ScrollViewer;
                scrollView?.ScrollToEnd();
            }
        }
        public static bool GetAutoScroll(DependencyObject d)
        {
            return (bool)d.GetValue(AutoScrollProperty);
        }
        public static void SetAutoScroll(DependencyObject d, bool value)
        {
            d.SetValue(AutoScrollProperty, value);
        }
    }
}
