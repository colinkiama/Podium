using Podium.Model;
using ProductHuntClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        public List<Product> products { get; set; } = new List<Product>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {

            bool hasAuthorized = await _client.AuthorizeAsync();
            Debug.WriteLine($"Authorization Successful: {hasAuthorized}");
        }



        private async Task ShowTopPostsAsync()
        {
            var topPosts = await _client.GetTopPostsAsync();
            var topPostsList = (List<PHPost>)topPosts;
            products.Clear();
            for (int i = 0; i < topPosts.Count; i++)
            {
                products.Add(new Product(topPostsList[i], i + 1));
            }

            UpdateProductControls();
        }

        private void UpdateProductControls()
        {
            Product1.DataContext = products[0];
            Product2.DataContext = products[1];
            Product3.DataContext = products[2];
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool isAuthorized = false;
            if (!_client.TokenExists)
            {
                bool hasAuthorised = await _client.AuthorizeAsync();
                if (!hasAuthorised)
                {
                    return;
                }
            }
            else
            {
                isAuthorized = true;
            }

            if (isAuthorized)
            {
                await ShowTopPostsAsync();
            }
        }

        private void NotificationsToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void NotificationsToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
