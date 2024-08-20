using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ObligatorioTTec.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.Maui.Controls.Compatibility;


namespace ObligatorioTTec.Views
{
    public partial class Mapa : ContentPage
    {
        private MapType _currentMapType = MapType.Street;

        public List<Sucursal> SucursalesGuardadas { get; set; } = new List<Sucursal>();
        public List<Pin> Pins { get; set; } = new List<Pin>();
        private Pin _pinSeleccionado;

        private bool _areButtonsVisible = false;

        private HttpClient _httpClient = new HttpClient();

        private bool _isEditMode = false;
        private Sucursal _sucursalSeleccionada;

        private string _apiKey = "5a1c8a969c15b857e1a2c055bad4011a";

        private void OnEditClicked(object sender, EventArgs e)
        {
            _isEditMode = true;
            // ... (configurar el mapa para edición)
        }

        private DBL _database;

        public Mapa(List<Sucursal> sucursalesGuardadas)
        {
            InitializeComponent();
            SucursalesGuardadas = sucursalesGuardadas;

            _pinSeleccionado = new Pin();
            _sucursalSeleccionada = new Sucursal();

            MessagingCenter.Subscribe<EditarSucursalPage, Sucursal>(this, "ActualizarPin", async (sender, sucursal) =>
            {
                // Actualizar la ubicación del pin en el mapa
                await ActualizarUbicacionPin(sucursal);
            });

            // Initialize the database connection
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");
            _database = new DBL(databasePath);

            mapaUbicacion.MapClicked += OnMapClicked;

            // Crear una instancia de CustomPin
            var customPin = new CustomPin
            {
                Label = "Custom Location",
                Address = "Some Address",
                Location = new Location(37.79752, -122.40183),
                ImageSource = ImageSource.FromFile("cinema.png")
            };

            // Añadir el pin personalizado al mapa
            mapaUbicacion.Pins.Add(customPin);

        }

        private async Task ActualizarUbicacionPin(Sucursal sucursal)
        {
            // Aquí debes implementar la lógica para actualizar el pin en el mapa,
            // obteniendo las coordenadas de la nueva dirección y moviendo el pin.

            // Ejemplo: usando una API de geocodificación para obtener las nuevas coordenadas:
            var location = await GeocodeAddress(sucursal.Direccion);
            if (location != null)
            {
                var pin = new Pin
                {
                    Label = sucursal.Nombre,
                    Address = sucursal.Direccion,
                    Location = location
                };

                // Remover el pin antiguo y agregar el nuevo
                mapaUbicacion.Pins.Clear();
                mapaUbicacion.Pins.Add(pin);
            }
        }

        private async Task<Location> GeocodeAddress(string address)
        {
            var apiKey = "AIzaSyCEhRh7u4e1w9pggZ6ffUhvxnOdm48WDWE";
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={apiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonConvert.DeserializeObject<GeocodeResponse>(response);

            var result = json.Results.FirstOrDefault();
            if (result != null && result.Geometry != null && result.Geometry.Location != null)
            {
                return new Location(result.Geometry.Location.Lat, result.Geometry.Location.Lng);
            }

            return null;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarSucursales();
            AjustarVistaDelMapa();

            var geolocalizacion = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            var location = await Geolocation.GetLocationAsync(geolocalizacion);

            if (location != null)
            {
                mapaUbicacion.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromMiles(10)));
            }

            foreach (var sucursal in SucursalesGuardadas)
            {
                var pin = new Pin
                {
                    Label = sucursal.Nombre,
                    Address = sucursal.Direccion,
                    Location = new Location(
                        double.Parse(sucursal.Ubicacion.Split(',')[0]),
                        double.Parse(sucursal.Ubicacion.Split(',')[1])
                    )
                };

                Pins.Add(pin);
            }

            mapaUbicacion.ItemsSource = Pins;
        }

        private async Task CargarSucursales()
        {
            // Get sucursales from database
            SucursalesGuardadas = await _database.GetSucursales();
            Pins.Clear();
            var sucursales = await _database.GetSucursales();
            foreach (var sucursal in SucursalesGuardadas)
            {
                var pin = new Pin
                {
                    Label = sucursal.Nombre,
                    Address = sucursal.Direccion,
                    Location = new Location(
                        double.Parse(sucursal.Ubicacion.Split(',')[0]),
                        double.Parse(sucursal.Ubicacion.Split(',')[1])
                    )
                };

                Pins.Add(pin);
            }
        }



        private async void BtnBuscarSucursalCercana_Clicked(object sender, EventArgs e)
        {
            var ubicacionActual = await ObtenerUbicacionActual();

            if (ubicacionActual != null)
            {
                var sucursalMasCercana = EncontrarSucursalMasCercana(ubicacionActual);

                if (sucursalMasCercana != null)
                {
                    // Calcular la distancia entre la ubicación actual y la sucursal más cercana
                    var distancia = CalcularDistancia(ubicacionActual, sucursalMasCercana);

                    // Asumimos una velocidad promedio de 5 km/h (caminando)
                    double velocidadPromedio = 5.0; // en km/h
                    var tiempoEstimado = distancia / velocidadPromedio;

                    // Mostrar la información de la sucursal más cercana junto con la distancia y el tiempo estimado
                    await DisplayAlert(
                        "Sucursal Más Cercana",
                        $"Nombre: {sucursalMasCercana.Nombre}\nDirección: {sucursalMasCercana.Direccion}\nDistancia: {distancia:F2} km\nTiempo estimado: {tiempoEstimado:F2} horas",
                        "OK"
                    );

                    // Opcional: Puedes agregar un pin en la sucursal más cercana y ajustar la vista del mapa
                    var pinMasCercana = new Pin
                    {
                        Label = sucursalMasCercana.Nombre,
                        Address = sucursalMasCercana.Direccion,
                        Location = new Location(
                            double.Parse(sucursalMasCercana.Ubicacion.Split(',')[0]),
                            double.Parse(sucursalMasCercana.Ubicacion.Split(',')[1])
                        )
                    };

                    mapaUbicacion.Pins.Clear(); // Limpiar pines existentes
                    mapaUbicacion.Pins.Add(pinMasCercana);

                    mapaUbicacion.MoveToRegion(MapSpan.FromCenterAndRadius(pinMasCercana.Location, Distance.FromMiles(1)));
                }
                else
                {
                    await DisplayAlert("Error", "No se encontraron sucursales.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicación actual.", "OK");
            }
        }

        // Método para calcular la distancia entre dos ubicaciones usando la fórmula de Haversine
        private double CalcularDistancia(Location ubicacionActual, Sucursal sucursal)
        {
            var lat1 = ubicacionActual.Latitude;
            var lon1 = ubicacionActual.Longitude;
            var lat2 = double.Parse(sucursal.Ubicacion.Split(',')[0]);
            var lon2 = double.Parse(sucursal.Ubicacion.Split(',')[1]);

            var radioTierra = 6371; // Radio de la tierra en kilómetros

            var dLat = GradosARadianes(lat2 - lat1);
            var dLon = GradosARadianes(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(GradosARadianes(lat1)) * Math.Cos(GradosARadianes(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distancia = radioTierra * c; // Distancia en kilómetros

            return distancia;
        }

        // Método para convertir grados a radianes
        private double GradosARadianes(double grados)
        {
            return grados * (Math.PI / 180);
        }



        private Sucursal EncontrarSucursalMasCercana(Location ubicacionActual)
        {
            Sucursal sucursalMasCercana = null;
            double distanciaMinima = double.MaxValue;

            foreach (var sucursal in SucursalesGuardadas)
            {
                // Asegúrate de que el formato de ubicacion sea correcto
                if (!sucursal.Ubicacion.Contains(","))
                {
                    Console.WriteLine("Formato de ubicación inválido para la sucursal: " + sucursal.Nombre);
                    continue; // Saltar a la siguiente sucursal
                }

                // Parsear latitud y longitud de forma segura
                if (double.TryParse(sucursal.Ubicacion.Split(',')[0], out double latitudSucursal) &&
                    double.TryParse(sucursal.Ubicacion.Split(',')[1], out double longitudSucursal))
                {
                    var ubicacionSucursal = new Location(latitudSucursal, longitudSucursal);

                    var distancia = CalcularDistancia(
                        ubicacionActual.Latitude,
                        ubicacionActual.Longitude,
                        ubicacionSucursal.Latitude,
                        ubicacionSucursal.Longitude);

                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        sucursalMasCercana = sucursal;
                    }
                }
                else
                {
                    Console.WriteLine("Error al parsear la ubicación de la sucursal: " + sucursal.Nombre);
                }
            }

            return sucursalMasCercana;
        }
        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            // Método simple para calcular la distancia entre dos puntos en la Tierra
            var dlat = ToRadians(lat2 - lat1);
            var dlon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distancia = 6371 * c; // Radio de la Tierra en kilómetros
            return distancia;
        }

        private double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            var newPin = new Pin
            {
                Label = "Ubicación seleccionada",
                Address = "Ubicación del pin",
                Location = new Location(e.Location.Latitude, e.Location.Longitude)
            };

            // Clear previous pins if needed
            Pins.Clear();
            Pins.Add(newPin);
            mapaUbicacion.Pins.Clear(); // Clear existing pins on the map
            foreach (var pin in Pins)
            {
                mapaUbicacion.Pins.Add(pin);
            }

            _pinSeleccionado = newPin;

            mapaUbicacion.MoveToRegion(MapSpan.FromCenterAndRadius(e.Location, Distance.FromMiles(1)));
        }

        private async void BtnGuardarPin_Clicked(object sender, EventArgs e)
        {
            if (_pinSeleccionado != null)
            {

                // Obtener la dirección a partir de la ubicación del pin
                var direcciones = await Geocoding.GetPlacemarksAsync(_pinSeleccionado.Location);
                var placemark = direcciones?.FirstOrDefault();

                // Crear una lista de calles
                var calles = new List<string>();

                if (placemark != null)
                {


                    if (!string.IsNullOrWhiteSpace(placemark.SubThoroughfare))
                        calles.Add(placemark.SubThoroughfare); // Subcalle

                    if (!string.IsNullOrWhiteSpace(placemark.AdminArea))
                        calles.Add(placemark.AdminArea);

                    if (!string.IsNullOrWhiteSpace(placemark.Thoroughfare))
                        calles.Add(placemark.Thoroughfare); // Calle principal
                }

                var nombre = await DisplayPromptAsync("Nombre", "Ingrese el nombre de la sucursal:");
                var direccion = string.Format("{0}, {1}, {2}", placemark.Thoroughfare, placemark.SubThoroughfare, placemark.AdminArea); // Unir calles en una cadena

                // Aquí podrías mostrar un dialogo o selección para elegir el ícono
                var iconoSeleccionado = "cinema.png"; // Establece un ícono por defecto o seleccionado

                if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(direccion))
                {
                    var nuevaSucursal = new Sucursal
                    {
                        Nombre = nombre,
                        Direccion = direccion,
                        Telefono = "", // Puedes agregar un campo para el teléfono si es necesario
                        Ubicacion = $"{_pinSeleccionado.Location.Latitude}, {_pinSeleccionado.Location.Longitude}",
                        Icono = iconoSeleccionado // Establece el ícono
                    };

                    bool success = await _database.SetSucursal(nuevaSucursal);

                    if (success)
                    {
                        await DisplayAlert("Éxito", "La sucursal se guardó correctamente.", "OK");
                        await CargarSucursales(); // Reload sucursales to reflect new pin
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo guardar la sucursal.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Primero debe seleccionar una ubicación en el mapa.", "OK");
            }
        }



        private async void BtnNavegar_Clicked(object sender, EventArgs e)
        {
            if (_pinSeleccionado != null)
            {
                await NavegarHaciaPin(_pinSeleccionado);
            }
            else
            {
                await DisplayAlert("Error", "No hay un pin seleccionado para navegar.", "OK");
            }
        }

        private async Task NavegarHaciaPin(Pin pinSeleccionado)
        {
            var ubicacionActual = await ObtenerUbicacionActual();

            if (ubicacionActual != null)
            {
                var latitude = ubicacionActual.Latitude;
                var longitude = ubicacionActual.Longitude;
                var destinoLatitude = pinSeleccionado.Location.Latitude;
                var destinoLongitude = pinSeleccionado.Location.Longitude;

                var url = $"https://www.google.com/maps/dir/?api=1&origin={latitude},{longitude}&destination={destinoLatitude},{destinoLongitude}";

                await Launcher.OpenAsync(url);
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicación actual.", "OK");
            }
        }

        private async Task<Location> ObtenerUbicacionActual()
        {
            try
            {
                var geolocalizacion = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                return await Geolocation.GetLocationAsync(geolocalizacion);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo obtener la ubicación actual: {ex.Message}", "OK");
                return null;
            }
        }

        private async void BtnVerSucursales_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VerSucursales());
        }

        private void BtnCambiarTipoMapa_Clicked(object sender, EventArgs e)
        {
            // Cambiar el tipo de mapa
            if (_currentMapType == MapType.Satellite)
            {
                mapaUbicacion.MapType = MapType.Hybrid;
                _currentMapType = MapType.Hybrid;
            }
            else if (_currentMapType == MapType.Hybrid)
            {
                mapaUbicacion.MapType = MapType.Street;
                _currentMapType = MapType.Street;
            }
            else
            {
                mapaUbicacion.MapType = MapType.Satellite;
                _currentMapType = MapType.Satellite;
            }
        }

        private void AjustarVistaDelMapa()
        {
            if (Pins.Count > 0)
            {
                var latitudes = Pins.Select(pin => pin.Location.Latitude).ToList();
                var longitudes = Pins.Select(pin => pin.Location.Longitude).ToList();
                var minLat = latitudes.Min();
                var maxLat = latitudes.Max();
                var minLong = longitudes.Min();
                var maxLong = longitudes.Max();

                var center = new Location((minLat + maxLat) / 2, (minLong + maxLong) / 2);
                var latitudeDegrees = maxLat - minLat;
                var longitudeDegrees = maxLong - minLong;

                var mapSpan = new MapSpan(center, latitudeDegrees, longitudeDegrees);
                mapaUbicacion.MoveToRegion(mapSpan);
            }
        }


        private void BtnToggle_Clicked(object sender, EventArgs e)
        {
            _areButtonsVisible = !_areButtonsVisible;
            BotonesStackLayout.IsVisible = _areButtonsVisible;
            BotonesStackLayoute.IsVisible = _areButtonsVisible;

            // Cambia la imagen y rotación del botón dependiendo del estado
            if (_areButtonsVisible)
            {
                BtnToggle.ImageSource = "left.png";
                BtnToggle.ScaleX = -1;
            }
            else if (!_areButtonsVisible)
            {
                BtnToggle.ImageSource = "left.png";
                BtnToggle.ScaleX = 1;
            }
        }


        private async void ActualizarClima(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();
                if (location != null)
                {
                    var latitude = location.Latitude;
                    var longitude = location.Longitude;

                    var apiKey = "5a1c8a969c15b857e1a2c055bad4011a";
                    var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(url);
                        var json = await response.Content.ReadAsStringAsync();
                        var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);

                        // Mostrar la temperatura, descripción y un icono del clima
                        ClimaLabel.Text = $"{weatherData.main.temp} °C";
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo obtener la ubicación.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
            }
        }



    }

    public class WeatherData
    {
        public Main main { get; set; }
        public List<Weather> weather { get; set; }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int humidity { get; set; }
            // ... otros datos relevantes
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; } // Descripción general del clima (e.g., Clouds, Rain)
            public string description { get; set; } // Descripción detallada del clima
            public string icon { get; set; } // Código del icono para la condición climática
        }
    }

    public class GeocodeResponse
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }

        public class Result
        {
            public Geometry Geometry { get; set; }
            public AddressComponent[] AddressComponents { get; set; }
            public string FormattedAddress { get; set; }
        }

        public class Geometry
        {
            public Location Location { get; set; }
        }

        public class Location
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class AddressComponent
        {
            public string LongName { get; set; }
            public string ShortName { get; set; }
            public string[] Types { get; set; }
        }
    }

   

}