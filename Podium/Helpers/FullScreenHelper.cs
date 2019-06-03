using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Podium.Helpers
{
    public sealed class FullScreenHelper
    {
        private TextBlock TitleBarTextBlock;

        public FullScreenHelper(TextBlock appNameTextBlock)
        {
            TitleBarTextBlock = appNameTextBlock;
            Window.Current.SizeChanged += WindowSizeChanged;
        }

        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {

            if (ApplicationView.GetForCurrentView().IsFullScreenMode)
            {
                TitleBarTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                TitleBarTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
