using Microsoft.Maui.Controls;

namespace AppLinkMauiApp
{
    [QueryProperty(nameof(UserId), "userId")]
    public partial class ProfilePage : ContentPage
    {
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(); // Notify property change
                // Handle the userId parameter (e.g., fetch user data)
                UserIdLabel.Text = $"User ID: {UserId}";
            }
        }
        private string _userId;

        public ProfilePage()
        {
            InitializeComponent();
        }
    }
}
