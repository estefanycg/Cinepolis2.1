using Xamarin.Forms;

public class CarouselItem
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Duracion { get; set; }
    public string Genero { get; set; }
    public string Sinopsis { get; set; }
    public string Imagen { get; set; }
    public string Actores { get; set; }
    public int Estado { get; set; }
    public string Clasificacion { get; set; }
    public ImageSource ImageSource { get; set; }
    public string Name => Titulo;
}