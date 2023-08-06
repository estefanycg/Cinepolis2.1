using System.ComponentModel;
using System.Globalization;

namespace Cinepolis.ModelViews
{

    public class Datos : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public string Pelicula { get; set; }

        public string ciudad { get; set; }

        public string fecha { get; set; }

        public string hora { get; set; }

        public int sala { get; set; }

        public string asientos { get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        public string correo { get; set; }

        public double total { get; set; }

        public string ImagenProducto { get; set; }

        public string aceptaTerminos { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
