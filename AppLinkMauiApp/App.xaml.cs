using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Dispatching;
using System;
using Sentry;

namespace AppLinkMauiApp;

public partial class App : Application
{
    // Fields to store deep link data
    private string _receivedUserId;
    private bool _receivedVerificationStatus;
    private bool _hasDeepLinkData = false;

    // Getters for the deep link data
    public string GetReceivedUserId() => _receivedUserId;
    public bool GetReceivedVerificationStatus() => _receivedVerificationStatus;
    public bool GetHasDeepLinkData() => _hasDeepLinkData;

    // Setter for the deep link data status
    public void SetHasDeepLinkData(bool value) => _hasDeepLinkData = value;

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell(); // Set the main page of the app to AppShell
    }

    // Override CreateWindow to manage window creation and activity handling
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        // Perform any other custom initialization if needed

        return window;
    }

    // Override to handle deep link requests
    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        try
        {
            // Parse the query parameters from the deep link URI
            var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
            string userId = queryParameters["userId"];
            string verificationStatus = queryParameters["verified"];

            // Check if the required parameters are present
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(verificationStatus))
            {
                // Store the received data
                _receivedUserId = userId;
                _receivedVerificationStatus = bool.Parse(verificationStatus);
                _hasDeepLinkData = true;

                // Update the UI on the main thread
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MainPage is AppShell appShell)
                    {
                        var mainPage = appShell.CurrentPage as MainPage;
                        if (mainPage != null)
                        {
                            // Display the verification info on the MainPage
                            mainPage.DisplayVerificationInfo(_receivedUserId, _receivedVerificationStatus);
                            SetHasDeepLinkData(false); // Clear data after use
                        }
                        else
                        {
                            Console.WriteLine("CurrentPage is not MainPage");
                        }
                    }
                    else
                    {
                        Console.WriteLine("MainPage is not AppShell");
                    }
                });
            }
            else
            {
                Console.WriteLine("Invalid or missing query parameters");
            }
        }
        catch (Exception ex)
        {
            // Log any exceptions using Sentry
            SentrySdk.CaptureException(ex);
            Console.WriteLine($"Error processing deep link: {ex.Message}");
        }
    }
}
