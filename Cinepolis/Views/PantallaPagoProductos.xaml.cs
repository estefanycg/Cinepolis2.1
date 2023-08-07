
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

    public partial class PantallaPagoProductos : ContentPage
    {
        public Pagos pago;
        public double Total { get; set; }
        public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public List<List<int>> golosinas;
        public double total;
        public string Mensaje;

        public PantallaPagoProductos(List<List<int>> golosinas_seleccionadas, double totalFacturar, string mensaje)
        {
            InitializeComponent();
            pago = new Pagos();
            Total = totalFacturar;
            golosinas = golosinas_seleccionadas;
            total = totalFacturar;
            Mensaje = mensaje;
            NavigationPage.SetHasNavigationBar(this, false);
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

        private async void PagoButtonClicked(object sender, EventArgs e)
        {
            try
            {

                var data = new
                {
                    golosinas,
                    total
                };

                string jsonData = JsonConvert.SerializeObject(data);

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "transacciones/facturar_productos");
                    request.Content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + (string)Application.Current.Properties["token"]);
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string mensaje = Nombre + ", gracias por tu compra!" + Mensaje + "\n\nTotal: L." + total.ToString();
                        await Navigation.PushAsync(new PantallaQR(mensaje));
                    }
                    else
                    {
                        await DisplayAlert("Error al facturar productos", "Intente De Nuevo", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void OnPagarButtonClicked(object sender, EventArgs e)
        {
            if (TerminosRadioButton.IsChecked)
            {
                // Aquí se puede realizar el proceso de pago y mostrar una confirmación o mensaje de éxito
                // Por ejemplo, mostrar un mensaje emergente con los datos de pago
                string mensaje = $"Pago realizado:\nNombres: {Nombre}\nCorreo: {Correo}\nTeléfono: {pago.NumeroTelefonico}\nCédula: {pago.CedulaIdentidad}";
                DisplayAlert("Pago Exitoso", mensaje, "Aceptar");
            }
            else
            {
                // Si faltan datos o no se han aceptado los términos, mostrar un mensaje de error
                DisplayAlert("Error", "Por favor, complete todos los campos y acepte los términos para continuar.", "Aceptar");
            }
        }


    }
}