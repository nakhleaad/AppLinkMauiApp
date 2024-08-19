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
            if (App.Current is App app && app.GetHasDeepLinkData())
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Display the information
                    DisplayVerificationInfo(app.GetReceivedUserId(), app.GetReceivedVerificationStatus());

                    // Clear the deep link data after handling it
                    app.SetHasDeepLinkData(false);
                });
            }
        }

        public void DisplayVerificationInfo(string userId, bool isVerified)
        {
            // Assuming you have Labels named UserIdLabel and VerificationStatusLabel in your XAML
            UserIdLabel.Text = $"User ID: {userId}";
            VerificationStatusLabel.Text = isVerified ? "Account Verified" : "Account Not Verified";
        }
    }






}
