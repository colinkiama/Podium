using Podium.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Podium.Controls
{
    public sealed partial class ProductControl : UserControl
    {
        public Product CurrentProduct { get { return this.DataContext as Product; }}
        public ProductControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private async void ViewProductButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(CurrentProduct.Post.Website));
        }

        private void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Flyout.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
