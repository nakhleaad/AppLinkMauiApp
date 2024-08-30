using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging;

namespace AppLinkMauiApp;

public static class MauiProgram
{
    // Static field to store the last processed deep link URI
    private static Uri _lastHandledUri;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>() // Sets up the MAUI app class as the entry point

            // Configures Sentry for crash reporting and performance monitoring
            .UseSentry(options =>
            {
                options.Dsn = "https://68eaf091b4b3f04203918134be1843cb@o4507847940505600.ingest.de.sentry.io/4507853269499984";
                options.Debug = true;
                options.TracesSampleRate = 1.0; // 100% of traces will be sent
            })

            // Configures custom fonts for the app
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })

            // Configures lifecycle events for different platforms
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                // Adds lifecycle events specific to Android
                lifecycle.AddAndroid(android =>
                {
                    android.OnCreate((activity, bundle) =>
                    {
                        var action = activity.Intent?.Action;
                        var data = activity.Intent?.Data?.ToString();

                        if (action == Android.Content.Intent.ActionView && data is not null)
                        {
                            // Handle the deep link on a background thread to avoid blocking the UI thread
                            Task.Run(() => HandleAppLink(data));
                        }
                    });
                });
#endif
            });

#if DEBUG
        // Adds debug logging in development mode
        builder.Logging.AddDebug();
#endif

        return builder.Build(); // Builds and returns the MAUI app instance
    }

    // Method to handle the deep link URL
    static void HandleAppLink(string url)
    {
        // Attempts to create a URI from the provided URL
        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
        {
            // Check if the deep link has already been processed to avoid duplication
            if (!IsDeepLinkHandled(uri))
            {
                // Sends the URI to the app's deep link handler
                App.Current?.SendOnAppLinkRequestReceived(uri);

                // Store the URI as the last handled deep link
                _lastHandledUri = uri;
            }
        }
    }

    // Method to check if the deep link has already been handled
    static bool IsDeepLinkHandled(Uri uri)
    {
        // Compare the incoming URI with the last handled URI
        return _lastHandledUri != null && _lastHandledUri == uri;
    }
}
