using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Cinepolis.ModelViews;


public class DetalleFactura : PopupPage
{
    private List<Golosina> productos;

    public DetalleFactura(List<Golosina> productos)
    {
        this.productos = productos;

        var stackLayout = new StackLayout
        {
            BackgroundColor = Color.White,
            Padding = new Thickness(20),
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 10,
            // Establece el radio de los bordes
            WidthRequest = 300
        };

        stackLayout.Children.Add(new Label
        {
            Text = "Detalle de la compra",
            FontSize = 25,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Color.Black
        });

        bool anyProductWithQuantity = false; // Variable para verificar si hay algún producto con cantidad mayor a 0

        // Agrega los detalles de cada producto a la ventana emergente
        foreach (var producto in productos)
        {
            if (producto.Cantidad > 0) // Verifica si la cantidad es mayor a 0
            {
                anyProductWithQuantity = true; // Hay al menos un producto con cantidad mayor a 0
                stackLayout.Children.Add(new Label
                {
                    Text = $"HN-{producto.Nombre} X {producto.Cantidad}      L. {producto.Total}",
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });

                stackLayout.Children.Add(new Label
                {
                    Text = $"Precio unitario: L. {producto.Precio}",
                    FontSize = 18,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                });

                stackLayout.Children.Add(new BoxView
                {
                    Color = Color.Black,
                    HeightRequest = 1,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                });
            }
        }

        if (anyProductWithQuantity)
        {
            // Agrega el total general de la compra solo si hay algún producto con cantidad mayor a 0
            stackLayout.Children.Add(new Label
            {
                Text = $"Total a pagar: L. {Total}",
                FontSize = 20,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            });
        }
        else
        {
            // Agrega un mensaje indicando que no hay productos con cantidad mayor a 0
            stackLayout.Children.Add(new Label
            {
                Text = "No hay productos en la compra.",
                FontSize = 20,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            });
        }

        Content = stackLayout;
    }

    public double Total
    {
        get
        {
            double total = 0;
            foreach (var producto in productos)
            {
                total += producto.Total;
            }
            return total;
        }
    }
}
