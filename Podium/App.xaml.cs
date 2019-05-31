using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Services.Store.Engagement;

namespace Podium
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        UISettings _uiSettings = new UISettings();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            _uiSettings.ColorValuesChanged += ColorValuesChanged;
        }

        private async void ColorValuesChanged(UISettings sender, object args)
        {

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
            () =>
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                var colorValue = sender.GetColorValue(UIColorType.Background);
                bool isDarkMode = colorValue == Colors.Black;
                Color colorToUse;
                if (isDarkMode)
                {
                    colorToUse = Color.FromArgb(255, 23, 23, 23);

                }
                else
                {
                    colorToUse = Colors.White;

                }
                titleBar.BackgroundColor = colorToUse;
                titleBar.BackgroundColor = colorToUse;
                titleBar.ButtonBackgroundColor = colorToUse;
                titleBar.InactiveBackgroundColor = colorToUse;
                titleBar.ButtonInactiveBackgroundColor = colorToUse;
            });

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            PrepareAppWindow();

            // Register for Targeted notifications
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            _ = engagementManager.RegisterNotificationChannelAsync();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private void PrepareAppWindow()
        {
            var appView = ApplicationView.GetForCurrentView();
            var pageBackgroundThemeBrush = (SolidColorBrush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"];
            var colorToUse = pageBackgroundThemeBrush.Color;
            var titleBar = appView.TitleBar;
            titleBar.BackgroundColor = colorToUse;
            titleBar.ButtonBackgroundColor = colorToUse;
            titleBar.InactiveBackgroundColor = colorToUse;
            titleBar.ButtonInactiveBackgroundColor = colorToUse;

            appView.SetPreferredMinSize(new Size(500, 500));
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
