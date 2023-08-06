using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cinepolis.ModelViews
{
    public  class Golosina : INotifyPropertyChanged
    {

        public string Id { get; set; }
        public ImageSource ImagenCargada { get; set; }


        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public double Precio { get; set; }

        public string Imagen { get; set; }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set
            {
                if (cantidad != value)
                {
                    cantidad = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Total => Precio * Cantidad; // Calculamos el total según la cantidad y el precio

        // Comando para disminuir la cantidad
        public ICommand DecrementCommand => new Command(() =>
        {
            if (Cantidad > 0)
            {
                Cantidad--;
            }
        });

        // Comando para aumentar la cantidad
        public ICommand IncrementCommand => new Command(() =>
        {
            Cantidad++;
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

