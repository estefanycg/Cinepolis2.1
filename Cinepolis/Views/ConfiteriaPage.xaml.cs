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
            BindingContext = this;
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
            await Navigation.PushAsync(new DatosPersonales(new List<string> { }, 1, 2, 1.0f));
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
