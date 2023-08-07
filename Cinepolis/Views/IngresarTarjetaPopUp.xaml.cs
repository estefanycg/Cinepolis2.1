using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Cinepolis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IngresarTarjetaPopUp : PopupPage
    {
        public IngresarTarjetaPopUp()
        {
            InitializeComponent();
        }

        private async void ContinuarPagoButton_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes acceder al valor del Entry (tarjetaEntry.Text)
            // y realizar el procesamiento necesario antes de cerrar el popup
            string numeroTarjeta = tarjetaEntry.Text;

            if (string.IsNullOrWhiteSpace(numeroTarjeta))
            {
                await DisplayAlert("Advertencia", "Debe ingresar un número de tarjeta para realizar su compra", "Aceptar");
                return; // No continúa con el procesamiento si no se ingresó un número de tarjeta
            }

            else
            {
                var PantallaPagoProductos = new PantallaPagoProductos();
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
