using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Location : ContentPage
    {
        private List<Ciudad> ciudades;

        public Location()
        {
            InitializeComponent();
            ciudades = new List<Ciudad>();
            LoadCiudadesFromServer();
        }

        private async void LoadCiudadesFromServer()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync("http://64.227.10.233/ciudades");
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        var ciudadesArray = data?.data?.ciudades;
                        if (ciudadesArray != null)
                        {
                            ciudades = JsonConvert.DeserializeObject<List<Ciudad>>(ciudadesArray.ToString());
                            genderPicker.ItemsSource = ciudades;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to fetch cities from the API", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred while fetching cities from the API", "OK");
            }
        }

        private void CarteleraButtonClicked(object sender, EventArgs e)
        {
            // Aquí puedes acceder al valor seleccionado del Picker
            var selectedCity = genderPicker.SelectedItem as Ciudad;
            if (selectedCity != null)
            {
                int selectedCityId = selectedCity.Id;
                string selectedCityName = selectedCity.Nombre;
                Application.Current.Properties["ciudadNombre"] = selectedCityName;
                Application.Current.Properties["ciudadId"] = selectedCityId;
                Application.Current.Properties["token"] = "c79975fcb5ee8d521f52daa3fceb9b3008f67ede";


                // Muestra un DisplayAlert con el Id de la ciudad seleccionada
                // Puedes utilizar el Id y el Nombre de la ciudad como necesites
                // Por ejemplo, puedes navegar a la página de Cartelera pasando ambos valores como parámetros.
                //Navigation.PushAsync(new AppShell(selectedCityId, selectedCityName));
                Application.Current.MainPage = new AppShell();
                //Navigation.PushAsync(new CarteleraPage(selectedCityId, selectedCityName));


            }  
            else
            {
                DisplayAlert("Error", "Por favor, seleccione una ciudad", "OK");
            }
        }

    }
}
