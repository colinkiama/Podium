using ProductHuntClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace PodiumNotifications
{
    using Microsoft.Toolkit.Uwp.Notifications;
    using Windows.UI.Notifications;

    public sealed class ToastNotifier : IBackgroundTask
    {
        const string ApiKey = "50bbfb1bd07adf7bff8b2b4fb0e78112ef0a223f445b01670e68fa856a385ff4";
        const string ApiSecret = "e7c795e719b8b6b77b3f0d7caf85b288c7266cd4e9e5b3211911a259ffe5d883";

        BackgroundTaskDeferral _deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            PHClient client = new PHClient(ApiKey, ApiSecret);
            var posts = await client.GetTopPostsAsync();
            SendToastBasedOnPosts((List<PHPost>)posts);
            _deferral.Complete();
        }

        private void SendToastBasedOnPosts(List<PHPost> posts)
        {
            int rnd = new Random().Next(0, 99999);
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            for (int i = posts.Count-1; i > -1; i--)
            {
                SendToastWithHeader(posts[i], rnd, currentTime, i+1);
            }

            
        }

        private void SendToastWithHeader(PHPost post, int rnd, DateTimeOffset currentTime, int rank)
        {
            var toastContent = new ToastContent()
            {
                Header = new ToastHeader($"{rnd}", string.Format("{0:t}", currentTime), "oof"),
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = $"{PickRankBasedOnIndex(rank)} - {post.Name}"
                },
                new AdaptiveText()
                {
                    Text = $"{post.Description}"
                }
            },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = $"{post.Thumbnail.Url}"
                        }
                    }
                },
                Launch = $"{post.Url}",
                ActivationType = ToastActivationType.Protocol
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        private string PickRankBasedOnIndex(int rank)
        {
            string rankToReturn = "unranked";

            switch (rank)
            {
                case 1:
                    rankToReturn = "1st 🥇";
                    break;
                case 2:
                    rankToReturn = "2nd 🥈";
                    break;
                case 3:
                    rankToReturn = "3rd 🥉";
                    break;
            }

            return rankToReturn;
        }
    }


}
