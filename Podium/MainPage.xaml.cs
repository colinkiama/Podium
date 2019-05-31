using Podium.Helpers;
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
using Windows.ApplicationModel.Background;
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

            NotificationsToggleButton.IsChecked = CheckIfNotificationsRegistered() ? true : false;
            UpdateNotifcationButtonContent();

            if (!_client.TokenExists)
            {
                await _client.AuthorizeAsync();
            }
            await ShowTopPostsAsync();

        }

        private bool CheckIfNotificationsRegistered()
        {
            return BackgroundTaskRegistration.AllTasks.Values.Where(p => p.Name == "PodiumNotifications").Count() > 0;
        }

        private void NotificationsToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            RegisterNotifications();
            UpdateNotifcationButtonContent();
            
        }

        private void UpdateNotifcationButtonContent()
        {
            bool isNotifcationsButtonChecked = NotificationsToggleButton.IsChecked.Value;
            TurnNotificationsOffStack.Visibility = isNotifcationsButtonChecked ? Visibility.Visible : Visibility.Collapsed;
            TurnNotificationsOnStack.Visibility = isNotifcationsButtonChecked ? Visibility.Collapsed : Visibility.Visible;
        }

        private void RegisterNotifications()
        {
            if (!CheckIfNotificationsRegistered())
            {
                var builder = new BackgroundTaskBuilder();

                builder.Name = "PodiumNotifications";
                builder.TaskEntryPoint = "PodiumNotifications.ToastNotifier";
                builder.SetTrigger(new TimeTrigger(60, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                BackgroundTaskRegistration bgTask = builder.Register();

                ToastHelper.SendNotifcationsEnabledToast();
            }
        }

        private void NotificationsToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UnregisterNotifications();
            UpdateNotifcationButtonContent();
        }

        private void UnregisterNotifications()
        {
            if (CheckIfNotificationsRegistered())
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    task.Value.Unregister(true);
                }
                ToastHelper.SendNotificationsDisabledToast();
            }
        }
    }
}
