using System;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage

    {
        private bool isPasswordVisible;
        private bool isPasswordVisibleC;

        public string EyeImageSource => isPasswordVisible ? "eyehide.svg" : "eye.svg";
        public string EyeImageSourceC => isPasswordVisibleC ? "eyehide.svg" : "eye.svg";
        public string email;


        public ChangePasswordPage(string email)
        {
            InitializeComponent();
            isPasswordVisible = false;
            isPasswordVisibleC = false;
            BindingContext = this;
            this.email = email;
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            contra.IsPassword = !isPasswordVisible;
            OnPropertyChanged(nameof(EyeImageSource));
        }
        private void TogglePasswordVisibilityC(object sender, EventArgs e)
        {
            isPasswordVisibleC = !isPasswordVisibleC;
            contra_verif.IsPassword = !isPasswordVisibleC;
            OnPropertyChanged(nameof(EyeImageSourceC));
        }


        private async void ChangePasswordClicked(object sender, EventArgs e)
        {
            try
            {
                string new_password1 = contra.Text;
                string new_password2 = contra_verif.Text;
                var data = new
                {
                    new_password1,
                    new_password2,
                    email
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth/cambio_contra");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        await Navigation.PushAsync(new SignInPage());
                    }
                    else
                    {
                        await DisplayAlert("Credenciales Incorrectas", "Intente De Nuevo", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}