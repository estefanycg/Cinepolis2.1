using Xamarin.Forms;

namespace Cinepolis.Effects
{
    public class CursorColorEffect : RoutingEffect
    {
        public Color CursorColor { get; set; }

        public CursorColorEffect() : base($"Cinepolis.{nameof(CursorColorEffect)}")
        {
        }
    }
}