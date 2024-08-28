using Microsoft.Maui.LifecycleEvents;
using Microsoft.Extensions.Logging;

namespace AppLinkMauiApp;

public static class MauiProgram
{
    // Entry point for creating the MAUI app
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>() // Sets up the MAUI app class as the entry point

            // Configures Sentry for crash reporting and performance monitoring
            .UseSentry(options =>
            {
                // The DSN (Data Source Name) is the only required setting for Sentry
                options.Dsn = "https://68eaf091b4b3f04203918134be1843cb@o4507847940505600.ingest.de.sentry.io/4507853269499984";

                // Enables debug mode to get more detailed Sentry logs in the console
                options.Debug = true;

                // Configures the sampling rate for performance monitoring
                options.TracesSampleRate = 1.0; // 1.0 means 100% of traces will be sent

                // Additional Sentry options can be configured here if needed
            })

            // Configures custom fonts for the app
            .ConfigureFonts(fonts =>
            {
                // Adds the OpenSans-Regular font with the alias "OpenSansRegular"
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

                // Adds the OpenSans-Semibold font with the alias "OpenSansSemibold"
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })

            // Configures lifecycle events for different platforms
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                // Adds lifecycle events specific to Android
                lifecycle.AddAndroid(android =>
                {
                    // Handles the OnCreate event for Android activities
                    android.OnCreate((activity, bundle) =>
                    {
                        // Retrieves the intent action and data (e.g., deep link URL)
                        var action = activity.Intent?.Action;
                        var data = activity.Intent?.Data?.ToString();

                        // If the action is a view action and data (URL) is not null, handle the deep link
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
            // Sends the URI to the app's deep link handler
            App.Current?.SendOnAppLinkRequestReceived(uri);
        }
    }
}
