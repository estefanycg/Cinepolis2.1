
using Cinepolis.ModelViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cinepolis.Views
{
	
	public partial class PantallaPagos : ContentPage
	{
		public Pagos pago;
        public float Total { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public int id_horario;
        List<string> asientos;
        public float total;
        public string aPagar { get; set; }
        public int id_sala;

		public PantallaPagos (int horario, List<string> asientosSeleccionado, float totalAsientos, string totalAPagar)
		{
			InitializeComponent ();
			pago = new Pagos ();
            Total = 500.00f;
            NavigationPage.SetHasNavigationBar(this, false);
            id_horario = horario;
            asientos = asientosSeleccionado;
            total = totalAsientos;
            aPagar = totalAPagar;
            id_sala = (int)Application.Current.Properties["id_sala"];
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPerfil();
            BindingContext = this;
        }


        public async Task LoadPerfil(){
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + Application.Current.Properties["token"]);
                    HttpResponseMessage response = await httpClient.GetAsync("http://64.227.10.233/auth/cliente");
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        Nombre = data?.data?.cliente?.first_name + " " + data?.data?.cliente?.last_name;
                        DNI = data?.data?.cliente?.DNI;
                        Correo = data?.data?.cliente?.email;
                        Telefono = data?.data?.cliente?.telefono;
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

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Aquí se puede navegar hacia atrás a la página anterior
            Navigation.PopAsync();
        }

        private async void OnTerminosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TerminosPage());
        }

        public async void PagoButtonClicked(object sender, EventArgs e)
        {
            try
            {

                var data = new
                {
                    id_horario,
                    asientos,
                    total,
                    id_sala
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + (string)Application.Current.Properties["token"]);
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "transacciones/facturar_boleto");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string mensaje = Nombre + ", gracias por tu compra!\n\nPelicula: " + (string)Application.Current.Properties["titulo"] + "\n\nHora: " + (string)Application.Current.Properties["hora"] + "\n\nSala: #" + (string)Application.Current.Properties["sala"] +  "\n\nAsientos: " + (string)Application.Current.Properties["asientos"] + "\n\nTotal: L." + (string)Application.Current.Properties["total"];
                        await Navigation.PushAsync(new PantallaQR(mensaje));
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
    }
}