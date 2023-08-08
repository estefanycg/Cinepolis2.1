using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cinepolis.ModelViews;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    public partial class Home : ContentPage
    {

        public List<Elemento> Elementos { get; set; }
        public string CiudadNombre { get; set; }

        public Home()
        {
            InitializeComponent();
            Elementos = new List<Elemento>();
            CiudadNombre = (string)Application.Current.Properties["ciudadNombre"];
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
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + (string)Application.Current.Properties["token"]);
                    HttpResponseMessage response = await httpClient.GetAsync("http://64.227.10.233/transacciones/facturas");
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        var facturas = data?.data?.facturas;

                        if (facturas != null)
                        {
                            foreach (var factura in facturas)
                            {
                                int id = factura["id"];
                                string fecha = (string)factura["fecha"];
                                double total = factura["total"];
                                Elementos.Add(new Elemento { numFactura = id, monto = total, fecha = fecha });
                            }
                        }
                        FacturasListView.ItemsSource = Elementos;
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

        public class Elemento
        {
            public int numFactura { get; set; }
            public double monto { get; set; }
            public string fecha { get; set; }
        }

    }
}
