using Newtonsoft.Json.Linq;
using ObligatorioTTec.Models;
using System.Collections.ObjectModel;


namespace ObligatorioTTec.Views
{
    public partial class EditarSucursalPage : ContentPage
    {
        private Sucursal _sucursal;
        private DBL _database;

        private HttpClient _httpClient = new HttpClient();

        public ObservableCollection<string> DireccionesSugeridas { get; set; } = new ObservableCollection<string>();

        public EditarSucursalPage(Sucursal sucursal)
        {
            InitializeComponent();
            BindingContext = this;  // Agregar esto para establecer el contexto de datos

            _sucursal = sucursal;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");
            _database = new DBL(databasePath);

            NombreEntry.Text = _sucursal.Nombre;
            DireccionEntry.Text = _sucursal.Direccion;
            TelefonoEntry.Text = _sucursal.Telefono;

            DireccionesListView.ItemsSource = DireccionesSugeridas;
            DireccionesListView.ItemTapped += OnDireccionSugeridaTapped;
        }

        public Command<string> BuscarDireccionCommand => new Command<string>(async (texto) =>
        {
            if (string.IsNullOrEmpty(texto))
                return;

            var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={texto}&key=TU_API_KEY";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            DireccionesSugeridas.Clear();
            foreach (var prediccion in json["predictions"])
            {
                DireccionesSugeridas.Add(prediccion["description"].ToString());
            }

            DireccionesListView.IsVisible = DireccionesSugeridas.Count > 0;
        });

        private void OnDireccionSugeridaTapped(object sender, ItemTappedEventArgs e)
        {
            var direccionSeleccionada = e.Item as string;
            DireccionEntry.Text = direccionSeleccionada;
            DireccionesListView.IsVisible = false;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _sucursal.Nombre = NombreEntry.Text;
            _sucursal.Direccion = DireccionEntry.Text;
            _sucursal.Telefono = TelefonoEntry.Text;

            var coordenadas = await ObtenerCoordenadasDesdeDireccion(_sucursal.Direccion);
            if (coordenadas != null)
            {
                _sucursal.Latitud = coordenadas.Item1;
                _sucursal.Longitud = coordenadas.Item2;
            }

            await _database.SetSucursal(_sucursal);
            await DisplayAlert("Éxito", "Sucursal actualizada correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async Task<Tuple<double, double>> ObtenerCoordenadasDesdeDireccion(string direccion)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={direccion}&key=AIzaSyCEhRh7u4e1w9pggZ6ffUhvxnOdm48WDWE";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            var location = json["results"]?[0]?["geometry"]?["location"];
            if (location != null)
            {
                var latitud = location["lat"].Value<double>();
                var longitud = location["lng"].Value<double>();
                return Tuple.Create(latitud, longitud);
            }

            return null;
        }

    }
}
