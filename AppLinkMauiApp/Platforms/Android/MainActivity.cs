using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;


namespace AppLinkMauiApp;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
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
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Handle the intent that started the activity, if applicable
        if (Intent?.Action == Intent.ActionView && Intent.Data != null)
        {
            try
            {
                HandleAppLink(Intent.Data.ToString()); // Handle the incoming deep link
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex); // Capture any exceptions that occur
            }
        }
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);

        // Handle the new intent that comes in after the activity is already running
        if (intent.Action == Intent.ActionView && intent.Data != null)
        {
            try
            {
                HandleAppLink(intent.Data.ToString()); // Handle the new deep link
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex); // Capture any exceptions that occur
            }
        }
    }

    static void HandleAppLink(string url)
    {
        try
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
                App.Current?.SendOnAppLinkRequestReceived(uri); // Notify the app about the deep link
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex); // Capture any exceptions that occur
        }
    }
}
