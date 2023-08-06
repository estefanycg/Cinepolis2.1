using System;
using System.Collections.Generic;
using System.Text;

namespace Cinepolis.ModelViews
{
    public class Producto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public double Precio { get; set; }
        public string Descripcion { get; set; }
        public string ImagenProducto { get; set; }
        public int Cantidad { get; set; }
        public double Total => Precio * Cantidad;
    }
}
