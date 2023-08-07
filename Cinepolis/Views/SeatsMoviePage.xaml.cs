using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cinepolis.ModelViews;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeatsMoviePage : ContentPage
    {

        public bool statusinvitado = InvitadoStatus.Value;
        private List<string> asientosSeleccionados = new List<string>();
        public Asientos asientos { get; set; }
        public int idHorario;
        public int pelicula;
        public float total;
        public int cantidadAsientos;

        public float precio { get; set; }
        public string titulo { get; set; }
        public string duracion { get; set; }
        public string genero { get; set; }
        public string clasificacion { get; set; }
        public string imagen { get; set; }



        public SeatsMoviePage(int idhorario, int idpelicula)
        {
            InitializeComponent();
            idHorario = idhorario;
            pelicula = idpelicula;
            asientos = new Asientos();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAsientos();
            BindingContext = this;
        }


        public async Task LoadAsientos()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await httpClient.GetAsync("servicios/asientos/" + idHorario);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(jsonResponse);
                        asientos.A1 = data?.data?.asientos?.A1;
                        asientos.A2 = data?.data?.asientos?.A2;
                        asientos.A3 = data?.data?.asientos?.A3;
                        asientos.A4 = data?.data?.asientos?.A4;
                        asientos.A5 = data?.data?.asientos?.A5;
                        asientos.A6 = data?.data?.asientos?.A6;

                        asientos.B1 = data?.data?.asientos?.B1;
                        asientos.B2 = data?.data?.asientos?.B2;
                        asientos.B3 = data?.data?.asientos?.B3;
                        asientos.B4 = data?.data?.asientos?.B4;
                        asientos.B5 = data?.data?.asientos?.B5;
                        asientos.B6 = data?.data?.asientos?.B6;

                        asientos.C1 = data?.data?.asientos?.C1;
                        asientos.C2 = data?.data?.asientos?.C2;
                        asientos.C3 = data?.data?.asientos?.C3;
                        asientos.C4 = data?.data?.asientos?.C4;
                        asientos.C5 = data?.data?.asientos?.C5;
                        asientos.C6 = data?.data?.asientos?.C6;
                        asientos.C7 = data?.data?.asientos?.C7;
                        asientos.C8 = data?.data?.asientos?.C8;

                        asientos.D1 = data?.data?.asientos?.D1;
                        asientos.D2 = data?.data?.asientos?.D2;
                        asientos.D3 = data?.data?.asientos?.D3;
                        asientos.D4 = data?.data?.asientos?.D4;
                        asientos.D5 = data?.data?.asientos?.D5;
                        asientos.D6 = data?.data?.asientos?.D6;
                        asientos.D7 = data?.data?.asientos?.D7;
                        asientos.D8 = data?.data?.asientos?.D8;
                        asientos.D9 = data?.data?.asientos?.D9;
                        asientos.D10 = data?.data?.asientos?.D10;

                        asientos.E1 = data?.data?.asientos?.E1;
                        asientos.E2 = data?.data?.asientos?.E2;
                        asientos.E3 = data?.data?.asientos?.E3;
                        asientos.E4 = data?.data?.asientos?.E4;
                        asientos.E5 = data?.data?.asientos?.E5;
                        asientos.E6 = data?.data?.asientos?.E6;
                        asientos.E7 = data?.data?.asientos?.E7;
                        asientos.E8 = data?.data?.asientos?.E8;
                        asientos.E9 = data?.data?.asientos?.E9;
                        asientos.E10 = data?.data?.asientos?.E10;

                        if (data?.data?.pelicula?.precio != null)
                        {
                            precio = data?.data?.pelicula?.precio;
                            titulo = data?.data?.pelicula?.titulo;
                            duracion = data?.data?.pelicula?.duracion;
                            genero = data?.data?.pelicula?.genero;
                            clasificacion = data?.data?.pelicula?.clasificacion;
                            imagen = "http://64.227.10.233" + data.data.pelicula.imagen;

                        }
                    }

                    else
                    {
                        await DisplayAlert("Error", "Failed to fetch data from the API", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred while fetching data from the API", "OK");
            }
        }

        private void OnSeatClicked(object sender, EventArgs e)
        {
            // Obtener el botón que activó el evento
            var button = (Button)sender;
            string seatNumber = button.Text;

            if (button.BackgroundColor == Color.Red)
            {
                DisplayAlert("Asiento ocupado", "Seleccione otro asiento, este ya ha sido ocupado.", "Aceptar");
                return;
            }

            // Cambiar el color del botón a naranja (#FFA500) si está azul (#3b5998)
            if (button.BackgroundColor == Color.FromHex("#3b5998"))
            {
                if (!asientosSeleccionados.Contains(seatNumber))
                {
                    // Si el asiento no está en el arreglo, lo agregamos
                    asientosSeleccionados.Add(seatNumber);
                }
                button.BackgroundColor = Color.FromHex("#FFA500");
            }
            else // Restaurar el color original si está naranja
            {
                button.BackgroundColor = Color.FromHex("#3b5998");
                // Si el botón está en color naranja (#FFA500)
                if (asientosSeleccionados.Contains(seatNumber))
                {
                    // Si el asiento está en el arreglo, lo eliminamos (se deselecciona)
                    asientosSeleccionados.Remove(seatNumber);
                }
            }


            // Actualizamos el contenido del label con los asientos seleccionados
            asientosArreglo.Text = string.Join(", ", asientosSeleccionados);
            cantidadAsientos = asientosSeleccionados.Count;
            asientosPrecioTotal.Text = "L. " + (cantidadAsientos * precio).ToString();


        }

        private void SiguienteClicked(System.Object sender, System.EventArgs e)
        {
            if (statusinvitado == true)
            {
                DisplayAlert("Iniciar Sesion", "Debe iniciar sesión para acceder.", "Aceptar");
                 Navigation.PushAsync(new SignInPage());
            }
            else
            {
                if (asientosSeleccionados.Count == 0)
                {
                     DisplayAlert("Advertencia", "Debes seleccionar al menos un asiento.", "Aceptar");
                }
                else
                {
                    Navigation.PushAsync(new DatosPersonales(asientosSeleccionados, idHorario, pelicula, total, (cantidadAsientos * precio).ToString()));
                }
            }

        }
    }

}