using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cinepolis.ModelViews;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieViewPage : ContentPage
    {
        public List<Horarios> HorariosPelicula { get; set; }

        public int pelicula;

        public string Titulo { get; set; } // Convertimos la variable titulo en una propiedad
        public string Duracion { get; set; } // Convertimos la variable titulo en una propiedad
        public string Genero { get; set; } // Convertimos la variable titulo en una propiedad
        public string Clasificacion { get; set; } // Convertimos la variable titulo en una propiedad
        public string Actores { get; set; } // Convertimos la variable titulo en una propiedad
        public string Sinopsis { get; set; } // Convertimos la variable titulo en una propiedad

        public ImageSource Imagen { get; set; } // Convertimos la variable titulo en una propiedad

        public MovieViewPage(int idpelicula)
        {

            pelicula = idpelicula;
            HorariosPelicula = new List<Horarios> { };

            //DisplayAlert("Imagen seleccionada", $"pelicula: {idpelicula}", $"ciudad: {idciudad}", "OK");

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPelicula();
            BindingContext = this;
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
                    HttpResponseMessage response = await httpClient.GetAsync("servicios/" + pelicula + "?ciudad=" + (int)Application.Current.Properties["ciudadId"]);
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
                                int idHorario = (int)horario["id"];
                                int idSala = (int)horario["sala"]["id"];
                                string horaInicio = (string)horario["hora_inicio"];
                                HorariosPelicula.Add(new Horarios { idhorario = idHorario, idsala = idSala, horainicio = horaInicio });
                            }
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

        public void OnSeleccionarClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int idHorario = (int)button.CommandParameter; // Obtener el ID del horario del CommandParameter
            string idHorarioString = idHorario.ToString(); // Convertir el ID a una cadena
            Navigation.PushAsync(new SeatsMoviePage(idHorario, pelicula));
        }
    }
}