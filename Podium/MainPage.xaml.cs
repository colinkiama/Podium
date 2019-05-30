using ProductHuntClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Podium
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PHClient _client = new PHClient(Constants.ApiKey, Constants.ApiSecret);
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            
            bool hasAuthorized = await _client.AuthorizeAsync();
            Debug.WriteLine($"Authorization Successful: {hasAuthorized}");
        }

        private void TopPostsButton_Click(object sender, RoutedEventArgs e)
        {
            var topPosts = _client.GetTopPostsAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AuthorizeButton.Visibility = _client.TokenExists ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
