using Cinepolis.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace Cinepolis
{
    public partial class SignUpPage : ContentPage
    {

        private bool isPasswordVisible;
        private bool isPasswordVisibleC;

        public string EyeImageSource => isPasswordVisible ? "eyehide.svg" : "eye.svg";
        public string EyeImageSourceC => isPasswordVisibleC ? "eyehide.svg" : "eye.svg";
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            isPasswordVisible = false;
            isPasswordVisibleC = false;
            BindingContext = this;
        }

        private async void OnSignInLinkClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }
        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            // Lógica para alternar la visibilidad de la contraseña
            isPasswordVisible = !isPasswordVisible;
            contra.IsPassword = !isPasswordVisible;
            OnPropertyChanged(nameof(EyeImageSource));
        }
        private void TogglePasswordVisibilityC(object sender, EventArgs e)
        {
            // Lógica para alternar la visibilidad de la contraseña
            isPasswordVisibleC = !isPasswordVisibleC;
            contraConf.IsPassword = !isPasswordVisibleC;
            OnPropertyChanged(nameof(EyeImageSourceC));
        }

        private async void RegistroButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string first_name = nombre.Text;
                string last_name = apellido.Text;
                string email = correo.Text;
                string password1 = contra.Text;
                string password2 = contraConf.Text;
                string DNI = dni.Text;
                string telefono = tel.Text;

                var data = new
                {
                    email,
                    password1,
                    password2,
                    first_name,
                    last_name,
                    DNI,
                    telefono,
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth/registro");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        await Navigation.PushAsync(new CodeVerifyEmailPage());
                    }
                    else
                    {
                        await DisplayAlert("Proveer todos los datos", "Intente De Nuevo", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async void SendCodeEmailClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CodeSignUpPage());
        }

    }
}