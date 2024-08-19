using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

namespace AppLinkMauiApp;

public partial class App : Application
{
    private string _receivedUserId;
    private bool _receivedVerificationStatus;
    private bool _hasDeepLinkData = false;

    public string GetReceivedUserId() => _receivedUserId;
    public bool GetReceivedVerificationStatus() => _receivedVerificationStatus;
    public bool GetHasDeepLinkData() => _hasDeepLinkData;

    public void SetHasDeepLinkData(bool value) => _hasDeepLinkData = value;

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
        string userId = queryParameters["userId"];
        string verificationStatus = queryParameters["verified"];

        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(verificationStatus))
        {
            _receivedUserId = userId;
            _receivedVerificationStatus = bool.Parse(verificationStatus);
            _hasDeepLinkData = true;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (MainPage is AppShell appShell)
                {
                    var mainPage = appShell.CurrentPage as MainPage;
                    if (mainPage != null)
                    {
                        mainPage.DisplayVerificationInfo(_receivedUserId, _receivedVerificationStatus);
                        SetHasDeepLinkData(false); // Clear data after use
                    }
                }
            });
        }
        else
        {
            Console.WriteLine("Invalid or missing query parameters");
        }
    }
}


