using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresarTarjetaPopUp : PopupPage
    {
        public List<List<int>> golosinas;
        public double total;
        public string Mensaje;

        public IngresarTarjetaPopUp(List<List<int>> golosinas_seleccionadas, double totalFacturar, string mensaje)
        {
            InitializeComponent();
            golosinas = golosinas_seleccionadas;
            total = totalFacturar;
            Mensaje = mensaje;
        }

        private async void ContinuarPagoButton_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes acceder al valor del Entry (tarjetaEntry.Text)
            // y realizar el procesamiento necesario antes de cerrar el popup
            string numeroTarjeta = tarjetaEntry.Text;

            if (string.IsNullOrWhiteSpace(numeroTarjeta))
            {
                await DisplayAlert("Advertencia", "Debe ingresar un número de tarjeta para realizar su compra", "Aceptar");
                return;
            }

            else
            {
                var PantallaPagoProductos = new PantallaPagoProductos(golosinas, total, Mensaje);
                await Navigation.PushAsync(PantallaPagoProductos);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async void CerrarButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
