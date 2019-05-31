using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Podium.Helpers
{
    public sealed class ToastHelper
    {
        const string enabledNotificationMessage = "Top 3 products will be shown here every hour! 🥳";
        const string disabledNotificationMessage = "Top 3 products will no longer be shown here. 😢";
        internal static void SendNotifcationsEnabledToast()
        {
            SendSimpleToast(enabledNotificationMessage);

        }

        internal static void SendNotificationsDisabledToast()
        {
            SendSimpleToast(disabledNotificationMessage);
        }

        private static void SendSimpleToast(string toastMessage)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = $"{toastMessage}"
                },
                    }
                    },
                }

            };


            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            toastNotif.Dismissed += ToastNotif_Dismissed;

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        private static void ToastNotif_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            if (args.Reason == ToastDismissalReason.UserCanceled || args.Reason == ToastDismissalReason.TimedOut)
            {
                ToastNotificationManager.CreateToastNotifier().Hide(sender);
            }
        }
    }
}
