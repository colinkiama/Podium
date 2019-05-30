﻿using ProductHuntClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace PodiumNotifications
{
    public sealed class ToastNotifier: IBackgroundTask
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
            throw new NotImplementedException();
        }
    }

    
}
