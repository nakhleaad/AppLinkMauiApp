namespace AppLinkMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override async void OnAppLinkRequestReceived(Uri uri)
    {
        base.OnAppLinkRequestReceived(uri);

        // Show an alert to test that the app link was received.
        await Dispatcher.DispatchAsync(async () =>
        {
            await Windows[0].Page!.DisplayAlert("App link received", uri.ToString(), "OK");
        });

        Console.WriteLine("App link: " + uri.ToString());
    }
}

//using Microsoft.Maui.Controls;

//namespace AppLinkMauiApp
//{
//    public partial class App : Application
//    {
//        public App()
//        {
//            InitializeComponent();
//            // Remove MainPage assignment as it's handled in CreateWindow
//        }

//        protected override async void OnAppLinkRequestReceived(Uri uri)
//        {
//            base.OnAppLinkRequestReceived(uri);

//            // Log the URI for debugging
//            Console.WriteLine("App link received: " + uri.ToString());

//            // Parse the query parameters
//            //var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
//            //var userId = query["userId"];

//            //// Navigate to the ProfilePage with the userId parameter
//            //if (!string.IsNullOrEmpty(userId))
//            //{
//            //    await Shell.Current.GoToAsync($"ProfilePage?userId={userId}");
//            //}
//            //else
//            //{
//            // Handle cases where userId is not provided or invalid
//            await Shell.Current.GoToAsync("//MainPage"); // Navigate to a default page
//            //}
//        }

//        //protected override Window CreateWindow(IActivationState activationState)
//        //{
//        //    return new MainPageWindow(); // Return the main window with AppShell
//        //}
//    }

//    //public class MainPageWindow : Window
//    //{
//    //    public MainPageWindow() : base(new AppShell()) { }
//    //}
//}
