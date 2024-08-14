using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Dispatching;

namespace AppLinkMauiApp;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    //LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.KeyboardHidden |
        ConfigChanges.Density)]
[IntentFilter(
    new string[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "https",
    DataHost = "mauiapp2024.web.app",
    AutoVerify = true)]
public class MainActivity : MauiAppCompatActivity
{
    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //base.OnCreate(savedInstanceState);

    //var intent = Intent;
    //if (intent?.Data != null)
    //{
    //    var data = intent.Data;
    //    var userId = data.GetQueryParameter("userId");
    //    var action = data.GetQueryParameter("action");

    //    // Ensure the navigation call is on the main thread
    //    if (action == "viewProfile" && !string.IsNullOrEmpty(userId))
    //    {
    //        MainThread.BeginInvokeOnMainThread(() => NavigateToProfilePage(userId));
    //    }
    //}
    //}

    //private void NavigateToProfilePage(string userId)
    //{
    //    // Ensure the navigation happens on the main thread
    //    MainThread.BeginInvokeOnMainThread(async () =>
    //    {
    //        // Fetch the navigation service on the main thread
    //        var navService = MauiApplication.Current.Services.GetService<INavigationService>();

    //        // Perform navigation on the main thread
    //        await Shell.Current.GoToAsync($"ProfilePage?userId={userId}");
    //    });
    //}
}
