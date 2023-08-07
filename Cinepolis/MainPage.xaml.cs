using Cinepolis.ModelViews;
using Cinepolis.Views;
using System;
using Xamarin.Forms;

namespace Cinepolis.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            BindingContext = this;
        }

        private async void OnSignInButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void CarteleraButtonClicked(object sender, EventArgs e)
        {
            InvitadoStatus.Value = true;
            await Navigation.PushAsync(new Location());

        }
    }
}