using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {

        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public UserProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPerfil();
            BindingContext = this;
        }

        public async Task LoadPerfil()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + (string) Application.Current.Properties["token"]);
                    HttpResponseMessage response = await httpClient.GetAsync("http://64.227.10.233/auth/cliente");
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        Nombre = data?.data?.cliente?.first_name + " " + data?.data?.cliente?.last_name;
                        DNI = data?.data?.cliente?.DNI;
                        Correo = data?.data?.cliente?.email;
                        Telefono = data?.data?.cliente?.telefono;
                        Direccion = data?.data?.cliente?.direccion;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to fetch perfil from the API", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred while fetching cities from the API", "OK");
            }
        }


    }
}