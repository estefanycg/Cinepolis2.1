using Cinepolis.ModelViews;
using Cinepolis.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cinepolis.Views
{
    public partial class DatosPersonales : ContentPage
    {
        public Datos datos; // Instancia de la clase Datos
        public List<string> asientos;
        public int Horario { get; set; }
        public int Pelicula { get; set; }
        public float Total { get; set; }
        public string Token { get; set; }
        public string Selecciones { get; set; }
        public int CiudadId { get; set; }
        public string CiudadNombre { get; set; }
        public string Fecha { get; set; }
        public int idSala { get; set; }
        public string Hora { get; set; }

        public string Titulo { get; set; }
        public string Duracion { get; set; }
        public string Genero { get; set; } 
        public string Clasificacion { get; set; } 
        public string Actores { get; set; } 
        public string Sinopsis { get; set; } 
        public ImageSource Imagen { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public DatosPersonales(List<string> lista, int id_horario, int id_pelicula, float asientos_total, string aPagar)
        {
            InitializeComponent();
            asientos = lista;
            Horario = id_horario;
            Pelicula = id_pelicula;
            Total = asientos_total;
            Fecha = "Lunes, 7/8/2023";
            totalAPagar.Text = aPagar;


            CiudadNombre = Application.Current.Properties["ciudadNombre"] as string;
            CiudadId = (int)Application.Current.Properties["ciudadId"];
            Token = Application.Current.Properties["token"] as string;

            if (asientos.Count > 0)
            {
                foreach (string seleccion in asientos)
                {
                    Selecciones += seleccion + ", ";
                }
            }
            Application.Current.Properties["asientos"] = Selecciones;
            Application.Current.Properties["total"] = Total;
            Application.Current.Properties["total"] = aPagar;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPelicula();
            await LoadPerfil();
            BindingContext = this;
        }

        public async Task LoadPerfil()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Token " + Token);
                    HttpResponseMessage response = await httpClient.GetAsync("http://64.227.10.233/auth/cliente");
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        Nombre = data?.data?.cliente?.first_name;
                        Apellido = data?.data?.cliente?.last_name;
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


        public async Task LoadPelicula()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://64.227.10.233/");
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await httpClient.GetAsync("servicios/" + Pelicula + "?ciudad=" + CiudadId);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                        Titulo = data?.data?.pelicula?.titulo;
                        Duracion = "Duracion: " + data?.data?.pelicula?.duracion + "Min";
                        Genero = "Genero: " + data?.data?.pelicula?.genero;
                        Clasificacion = "Clasificacion: " + data?.data?.pelicula?.clasificacion;
                        Actores = data?.data?.pelicula?.actores;
                        Sinopsis = data?.data?.pelicula?.sinopsis;
                        string img = "http://64.227.10.233" + data?.data?.pelicula?.imagen;
                        Imagen = ImageSource.FromUri(new Uri(img));

                        JArray horarioArray = data?.horarios;

                        if (horarioArray != null)
                        {
                            foreach (var horario in horarioArray)
                            {
                                if ((int)horario["id"] == Horario)
                                {
                                    Hora = (string)horario["hora_inicio"];
                                    idSala = (int)(horario["sala"]["id"]);
                                    Application.Current.Properties["hora"] = Hora;
                                    Application.Current.Properties["sala"] = idSala.ToString();
                                    Application.Current.Properties["id_sala"] = idSala;
                                }
                            }
                        }
                        Application.Current.Properties["titulo"] = Titulo;
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

        private void BOnSiguienteButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PantallaPagos(Horario, asientos, Total, totalAPagar.Text));

        }
    }
}
