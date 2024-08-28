using Microsoft.Maui.Dispatching;
using Sentry;

namespace AppLinkMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Use the dispatcher to ensure UI updates are on the main thread
            try
            {
                if (App.Current is App app && app.GetHasDeepLinkData())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            // Display the information received from the deep link
                            DisplayVerificationInfo(app.GetReceivedUserId(), app.GetReceivedVerificationStatus());

                            // Clear the deep link data after handling it
                            app.SetHasDeepLinkData(false);
                        }
                        catch (Exception ex)
                        {
                            SentrySdk.CaptureException(ex); // Capture any exceptions during UI update
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex); // Capture any exceptions outside of the main thread operation
            }
        }

        public void DisplayVerificationInfo(string userId, bool isVerified)
        {
            try
            {
                // Assuming you have Labels named UserIdLabel and VerificationStatusLabel in your XAML
                UserIdLabel.Text = $"User ID: {userId}";
                VerificationStatusLabel.Text = isVerified ? "Account Verified" : "Account Not Verified";
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex); // Capture exceptions related to UI update
            }
        }
    }
}
