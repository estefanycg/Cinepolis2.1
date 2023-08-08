using System.Collections.Generic;
using System;
using Cinepolis.ModelViews;
using Cinepolis.Views;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Net.Http;
using Newtonsoft.Json;

namespace Cinepolis.Views
{
    public partial class ConfiteriaPage : ContentPage
    {
        public List<Golosina> ConfiteriaList { get; set; }
        public string CiudadNombre { get; set; }
        public List<List<int>> golosinas;

        // Nueva propiedad para el total a pagar
        private double total;
        public double Total
        {
            get { return total; }
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public ConfiteriaPage()
        {
            InitializeComponent();
            LoadProductos(); // Llama al método para obtener los productos desde el servidor
            CiudadNombre = (string)Application.Current.Properties["ciudadNombre"];
            golosinas = new List<List<int>>();
            BindingContext = this;
        }


        public void agregarGolosinas()
        {
            foreach (var golosina in ConfiteriaList)
            {
                if (golosina.Cantidad > 0)
                {
                    List<int> valores = new List<int> { golosina.Id, golosina.Cantidad };
                    golosinas.Add(valores);
                }
            }
        }


        private async void LoadProductos()
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://64.227.10.233/productos");
                var data = JsonConvert.DeserializeObject<ProductosResponse>(response);

                if (data != null && data.Data?.Productos != null)
                {
                    ConfiteriaList = data.Data.Productos;
                    foreach (var producto in ConfiteriaList)
                    {
                        producto.PropertyChanged += CantidadChanged;
                        producto.ImagenCargada = ImageSource.FromUri(new Uri("http://64.227.10.233" + producto.Imagen));

                    }
                    UpdateTotal(); // Calcular el total al inicio
                    OnPropertyChanged(nameof(ConfiteriaList));
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Método para actualizar el total cada vez que cambie la cantidad de un producto
        private void UpdateTotal()
        {
            double total = 0;
            foreach (var producto in ConfiteriaList)
            {
                total += producto.Total;
            }
            Total = total;
        }

        // Evento que se dispara cuando cambia la cantidad de un producto
        private void CantidadChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Golosina.Cantidad))
            {
                UpdateTotal(); // Actualizar el total cuando cambie la cantidad de un producto
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            agregarGolosinas();
            if (Total <= 0)
            {
                await DisplayAlert("Sin productos seleccionados", "Debe seleccionar al menos un producto para realizar una compra.", "Aceptar");
            }
            else
            {
                //await Navigation.PushAsync(new PantallaPagoProductos(new List<string> { }, 1, 2, 1.0f));
                string mensajeLista = "";

                foreach (var producto in ConfiteriaList)
                {
                    if (producto.Cantidad > 0)
                    {
                        mensajeLista += "\n\n" + producto.Nombre + ": Cantidad " + producto.Cantidad;
                    }
                }

                if ((bool)Application.Current.Properties["invitado"])
                {
                    await DisplayAlert("Estas como invitado", "Por favor proveer tus credenciales para seguir", "Ok");
                    Navigation.PushAsync(new SignInPage());
                    return;
                }


                var ingresarTarjetaPopup = new IngresarTarjetaPopUp(golosinas, Total, mensajeLista);
                await PopupNavigation.Instance.PushAsync(ingresarTarjetaPopup);
            }

            
        }

        private async void CarritoIcon_Clicked(object sender, EventArgs e)
        {
            // Llama al constructor de la ventana emergente y pásale la lista de productos
            var detalleFacturaPopup = new DetalleFactura(ConfiteriaList);

            // Muestra la ventana emergente
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(detalleFacturaPopup);
        }
    }
}
