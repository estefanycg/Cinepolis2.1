using Cinepolis.Views;
using System;
using Xamarin.Forms;
//using Cinepolis.Servicios;
using System.Net.Http;
using Newtonsoft.Json;

namespace Cinepolis
{
    public partial class SignInPage : ContentPage
    {
        private bool isPasswordVisible; //

        public string EyeImageSource => isPasswordVisible ? "eyehide.svg" : "eye.svg"; //


        public SignInPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            isPasswordVisible = false; //
            BindingContext = this;

        }

        private async void OnSignUpLinkClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void ForgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async void SignInButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string email = correo.Text;
                string password = passwordEntry.Text;
                var data = new
                {
                    email,
                    password
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth/");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string respuesta = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(respuesta);
                        string token = responseObject?.data?.token;
                        if (!string.IsNullOrEmpty(token))
                        {
                            Application.Current.Properties["token"] = token;
                             await Navigation.PushAsync(new Location());
                        }
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

        private void TogglePasswordVisibility(object sender, EventArgs e) //
        {
            // Lógica para alternar la visibilidad de la contraseña
            isPasswordVisible = !isPasswordVisible;
            passwordEntry.IsPassword = !isPasswordVisible;
            OnPropertyChanged(nameof(EyeImageSource));
        }
    }
}
