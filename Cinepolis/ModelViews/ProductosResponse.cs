using System;
using System.Collections.Generic;
using System.Text;

namespace Cinepolis.ModelViews
{
    // Clase para representar la respuesta del servidor
    public class ProductosResponse
    {
        public ProductosData Data { get; set; }
    }

    // Clase para representar los datos de productos
    public class ProductosData
    {
        public List<Golosina> Productos { get; set; }
    }

}
