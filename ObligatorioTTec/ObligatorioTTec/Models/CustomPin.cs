using Microsoft.Maui.Controls.Maps;

namespace ObligatorioTTec.Models
{
    public class CustomPin : Pin
    {
        // Propiedad para especificar el color del Pin (esto es conceptual, la implementación depende del renderizador)
        public string PinColor { get; set; }

        // Propiedad para especificar una imagen personalizada para el Pin
        public string PinIcon { get; set; }

        // Propiedad para añadir cualquier información adicional
        public string Info { get; set; }

        public ImageSource ImageSource { get; set; }

        public View CustomView { get; set; }

        public CustomPin()
        {

        }
    }

    public class LocationBounds
    {
        public Location SouthWest { get; private set; }
        public Location NorthEast { get; private set; }

        public LocationBounds()
        {
            SouthWest = new Location(double.MaxValue, double.MaxValue);
            NorthEast = new Location(double.MinValue, double.MinValue);
        }

        public void Add(Location location)
        {
            SouthWest = new Location(
                Math.Min(SouthWest.Latitude, location.Latitude),
                Math.Min(SouthWest.Longitude, location.Longitude)
            );

            NorthEast = new Location(
                Math.Max(NorthEast.Latitude, location.Latitude),
                Math.Max(NorthEast.Longitude, location.Longitude)
            );
        }
    }
}
